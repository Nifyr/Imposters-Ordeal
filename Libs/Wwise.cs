using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static ImpostersOrdeal.Wwise;

namespace ImpostersOrdeal
{
    /// <summary>
    /// Handles deserialization and serialization of the Wwise .bnk format.
    /// </summary>
    public static class Wwise
    {

        public class WwiseData
        {
            internal byte[] buffer;
            internal int offset;
            public readonly Dictionary<uint, WwiseObject> objectsByID;
            public List<Bank> banks;

            public WwiseData()
            {
                objectsByID = new Dictionary<uint, WwiseObject>();
                banks = new();
            }

            public WwiseData(byte[] buffer)
            {
                this.buffer = buffer;
                offset = 0;
                objectsByID = new Dictionary<uint, WwiseObject>();
                banks = new();
                Bank b = new();
                b.Deserialize(this);
                banks.Add(b);
            }

            public void Parse(byte[] buffer)
            {
                this.buffer = buffer;
                offset = 0;
                Bank b = new();
                b.Deserialize(this);
                banks.Add(b);
            }

            public byte[] GetBytes()
            {
                return banks.First().Serialize().ToArray();
            }

            public byte[] GetBytes(uint id)
            {
                return objectsByID[id].Serialize().ToArray();
            }
        }

        public interface ISerializable
        {
            public abstract void Deserialize(WwiseData wd);
            public abstract IEnumerable<byte> Serialize();
        }

        public abstract class WwiseObject : ISerializable
        {
            public abstract void Deserialize(WwiseData wd);
            public abstract IEnumerable<byte> Serialize();
        }

        [JsonConverter(typeof(ChunkConverter))]
        public abstract class Chunk : ISerializable
        {
            public string tag;
            public uint chunkSize;

            public virtual void Deserialize(WwiseData wd)
            {
                tag = ReadString(wd, 4);
                chunkSize = ReadUInt32(wd);
            }

            public static Chunk Create(WwiseData wd)
            {
                string chunkType = Encoding.ASCII.GetString(wd.buffer, wd.offset, 4);
                Chunk bc = chunkType switch
                {
                    "BKHD" => new BankHeader(),
                    "DATA" => new DataChunk(),
                    "DIDX" => new MediaIndex(),
                    "ENVS" => new EnvSettingsChunk(),
                    "HIRC" => new HircChunk(),
                    "INIT" => new PluginChunk(),
                    "PLAT" => new CustomPlatformChunk(),
                    "STMG" => new GlobalSettingsChunk(),
                    _ => throw new NotImplementedException("ChunkType " + chunkType + " at " + wd.offset),
                };
                bc.Deserialize(wd);
                return bc;
            }

            public virtual IEnumerable<byte> Serialize()
            {
                List<byte> b = new();
                b.AddRange(GetBytes(tag));
                b.AddRange(GetBytes(chunkSize));
                return b;
            }
        }

        public class ChunkSpecifiedConcreteClassConverter : DefaultContractResolver
        {
            protected override JsonConverter ResolveContractConverter(Type objectType)
            {
                if (typeof(Chunk).IsAssignableFrom(objectType) && !objectType.IsAbstract)
                    return null; // pretend TableSortRuleConvert is not specified (thus avoiding a stack overflow)
                return base.ResolveContractConverter(objectType);
            }
        }

        public class ChunkConverter : JsonConverter
        {
            static JsonSerializerSettings SpecifiedSubclassConversion = new JsonSerializerSettings() { ContractResolver = new ChunkSpecifiedConcreteClassConverter() };

            public override bool CanConvert(Type objectType)
            {
                return (objectType == typeof(Chunk));
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                JObject jo = JObject.Load(reader);
                return jo["tag"].Value<string>() switch
                {
                    "BKHD" => JsonConvert.DeserializeObject<BankHeader>(jo.ToString(), SpecifiedSubclassConversion),
                    "DATA" => JsonConvert.DeserializeObject<DataChunk>(jo.ToString(), SpecifiedSubclassConversion),
                    "DIDX" => JsonConvert.DeserializeObject<MediaIndex>(jo.ToString(), SpecifiedSubclassConversion),
                    "ENVS" => JsonConvert.DeserializeObject<EnvSettingsChunk>(jo.ToString(), SpecifiedSubclassConversion),
                    "HIRC" => JsonConvert.DeserializeObject<HircChunk>(jo.ToString(), SpecifiedSubclassConversion),
                    "INIT" => JsonConvert.DeserializeObject<PluginChunk>(jo.ToString(), SpecifiedSubclassConversion),
                    "PLAT" => JsonConvert.DeserializeObject<CustomPlatformChunk>(jo.ToString(), SpecifiedSubclassConversion),
                    "STMG" => JsonConvert.DeserializeObject<GlobalSettingsChunk>(jo.ToString(), SpecifiedSubclassConversion),
                    _ => throw new NotImplementedException("Invalid tag: \"" + jo["tag"].Value<string>() + "\""),
                };
            }

            public override bool CanWrite
            {
                get { return false; }
            }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                throw new NotImplementedException(); // won't be called because CanWrite returns false
            }
        }

        [JsonConverter(typeof(HircItemConverter))]
        public abstract class HircItem : WwiseObject
        {
            public byte hircType;
            public uint sectionSize;
            public uint id;

            public override void Deserialize(WwiseData wd)
            {
                hircType = ReadUInt8(wd);
                sectionSize = ReadUInt32(wd);
                id = ReadUInt32(wd);
                wd.objectsByID[id] = this;
            }

            public static HircItem Create(WwiseData wd)
            {
                byte hircType = wd.buffer[wd.offset];
                HircItem hi = hircType switch
                {
                    1 => new State(),
                    2 => new Sound(),
                    3 => Action.GetInstance(wd),
                    4 => new Event(),
                    5 => new RanSeqCntr(),
                    6 => new SwitchCntr(),
                    7 => new ActorMixer(),
                    8 => new Bus(),
                    9 => new LayerCntr(),
                    10 => new MusicSegment(),
                    11 => new MusicTrack(),
                    12 => new MusicSwitchCntr(),
                    13 => new MusicRanSeqCntr(),
                    14 => new Attenuation(),
                    16 => new FxCustom(),
                    17 => new FxCustom(),
                    18 => new Bus(),
                    21 => new AudioDevice(),
                    _ => throw new NotImplementedException("HircType " + hircType + " at " + wd.offset),
                };
                hi.Deserialize(wd);
                return hi;
            }

            public override IEnumerable<byte> Serialize()
            {
                List<byte> b = new();
                b.Add(hircType);
                b.AddRange(GetBytes(sectionSize));
                b.AddRange(GetBytes(id));
                return b;
            }
        }

        public class HircItemSpecifiedConcreteClassConverter : DefaultContractResolver
        {
            protected override JsonConverter ResolveContractConverter(Type objectType)
            {
                if (typeof(HircItem).IsAssignableFrom(objectType) && !objectType.IsAbstract)
                    return null; // pretend TableSortRuleConvert is not specified (thus avoiding a stack overflow)
                return base.ResolveContractConverter(objectType);
            }
        }

        public class HircItemConverter : JsonConverter
        {
            static JsonSerializerSettings SpecifiedSubclassConversion = new JsonSerializerSettings() { ContractResolver = new HircItemSpecifiedConcreteClassConverter() };

            public override bool CanConvert(Type objectType)
            {
                return (objectType == typeof(HircItem));
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                JObject jo = JObject.Load(reader);
                return jo["hircType"].Value<byte>() switch
                {
                    1 => JsonConvert.DeserializeObject<State>(jo.ToString(), SpecifiedSubclassConversion),
                    2 => JsonConvert.DeserializeObject<Sound>(jo.ToString(), SpecifiedSubclassConversion),
                    3 => JsonConvert.DeserializeObject<Action>(jo.ToString(), SpecifiedSubclassConversion),
                    4 => JsonConvert.DeserializeObject<Event>(jo.ToString(), SpecifiedSubclassConversion),
                    5 => JsonConvert.DeserializeObject<RanSeqCntr>(jo.ToString(), SpecifiedSubclassConversion),
                    6 => JsonConvert.DeserializeObject<SwitchCntr>(jo.ToString(), SpecifiedSubclassConversion),
                    7 => JsonConvert.DeserializeObject<ActorMixer>(jo.ToString(), SpecifiedSubclassConversion),
                    8 => JsonConvert.DeserializeObject<Bus>(jo.ToString(), SpecifiedSubclassConversion),
                    9 => JsonConvert.DeserializeObject<LayerCntr>(jo.ToString(), SpecifiedSubclassConversion),
                    10 => JsonConvert.DeserializeObject<MusicSegment>(jo.ToString(), SpecifiedSubclassConversion),
                    11 => JsonConvert.DeserializeObject<MusicTrack>(jo.ToString(), SpecifiedSubclassConversion),
                    12 => JsonConvert.DeserializeObject<MusicSwitchCntr>(jo.ToString(), SpecifiedSubclassConversion),
                    13 => JsonConvert.DeserializeObject<MusicRanSeqCntr>(jo.ToString(), SpecifiedSubclassConversion),
                    14 => JsonConvert.DeserializeObject<Attenuation>(jo.ToString(), SpecifiedSubclassConversion),
                    16 => JsonConvert.DeserializeObject<FxCustom>(jo.ToString(), SpecifiedSubclassConversion),
                    17 => JsonConvert.DeserializeObject<FxCustom>(jo.ToString(), SpecifiedSubclassConversion),
                    18 => JsonConvert.DeserializeObject<Bus>(jo.ToString(), SpecifiedSubclassConversion),
                    21 => JsonConvert.DeserializeObject<AudioDevice>(jo.ToString(), SpecifiedSubclassConversion),
                    _ => throw new NotImplementedException("Invalid hircType: " + jo["hircType"].Value<byte>()),
                };
            }

            public override bool CanWrite
            {
                get { return false; }
            }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                throw new NotImplementedException(); // won't be called because CanWrite returns false
            }
        }

        public class Bank : WwiseObject
        {
            public List<Chunk> chunks;

            public override void Deserialize(WwiseData wd)
            {
                chunks = new();
                while (wd.offset < wd.buffer.Length)
                {
                    Chunk c = Chunk.Create(wd);
                    chunks.Add(c);
                    if (c is BankHeader bh)
                        wd.objectsByID[bh.akBankHeader.soundBankID] = this;
                }
            }

            public override IEnumerable<byte> Serialize()
            {
                List<byte> b = new();
                foreach (Chunk c in chunks)
                    b.AddRange(c.Serialize());
                return b;
            }
        }

        public class BankHeader : Chunk
        {
            public AkBankHeader akBankHeader;
            public int padding;

            public override void Deserialize(WwiseData wd)
            {
                base.Deserialize(wd);

                akBankHeader = new();
                int offset = wd.offset;
                akBankHeader.Deserialize(wd);
                padding = (int)(offset + chunkSize - wd.offset);
                wd.offset += padding;
            }

            public override IEnumerable<byte> Serialize()
            {
                List<byte> b0 = new();
                b0.AddRange(akBankHeader.Serialize());
                b0.AddRange(new byte[padding]);
                chunkSize = (uint)b0.Count;

                List<byte> b = new();
                b.AddRange(base.Serialize());
                b.AddRange(b0);
                return b;
            }
        }

        public class AkBankHeader : ISerializable
        {
            public uint bankGeneratorVersion;
            public uint soundBankID;
            public uint languageID;
            public ushort unused;
            public ushort deviceAllocated;
            public uint projectID;

            public void Deserialize(WwiseData wd)
            {
                bankGeneratorVersion = ReadUInt32(wd);
                soundBankID = ReadUInt32(wd);
                languageID = ReadUInt32(wd);
                unused = ReadUInt16(wd);
                deviceAllocated = ReadUInt16(wd);
                projectID = ReadUInt32(wd);
            }

            public IEnumerable<byte> Serialize()
            {
                List<byte> b = new();
                b.AddRange(GetBytes(bankGeneratorVersion));
                b.AddRange(GetBytes(soundBankID));
                b.AddRange(GetBytes(languageID));
                b.AddRange(GetBytes(unused));
                b.AddRange(GetBytes(deviceAllocated));
                b.AddRange(GetBytes(projectID));
                return b;
            }
        }

        public class MediaIndex : Chunk
        {
            public List<MediaHeader> loadedMedia;

            public override void Deserialize(WwiseData wd)
            {
                base.Deserialize(wd);

                loadedMedia = new();
                long endOffset = wd.offset + chunkSize;
                while (endOffset > wd.offset)
                {
                    MediaHeader mh = new();
                    mh.Deserialize(wd);
                    loadedMedia.Add(mh);
                }
            }

            public override IEnumerable<byte> Serialize()
            {
                List<byte> b = new();
                b.AddRange(GetBytes(tag));
                List<byte> loadedMediaBuffer = new();
                foreach (MediaHeader mediaHeader in loadedMedia)
                    loadedMediaBuffer.AddRange(mediaHeader.Serialize());
                chunkSize = (uint)loadedMediaBuffer.Count;
                b.AddRange(GetBytes(chunkSize));
                b.AddRange(loadedMediaBuffer);
                return b;
            }
        }

        public class MediaHeader : WwiseObject
        {
            public uint id;
            public uint offset;
            public uint size;

            public override void Deserialize(WwiseData wd)
            {
                id = ReadUInt32(wd);
                wd.objectsByID[id] = this;
                offset = ReadUInt32(wd);
                size = ReadUInt32(wd);
            }

            public override IEnumerable<byte> Serialize()
            {
                List<byte> b = new();
                b.AddRange(GetBytes(id));
                b.AddRange(GetBytes(offset));
                b.AddRange(GetBytes(size));
                return b;
            }
        }

        public class DataChunk : Chunk
        {
            public byte[] data;

            public override void Deserialize(WwiseData wd)
            {
                base.Deserialize(wd);

                data = ReadUInt8Array(wd, (int)chunkSize);
            }

            public override IEnumerable<byte> Serialize()
            {
                List<byte> b0 = new();
                b0.AddRange(data);
                chunkSize = (uint)b0.Count;

                List<byte> b = new();
                b.AddRange(base.Serialize());
                b.AddRange(b0);
                return b;
            }
        }

        public class EnvSettingsChunk : Chunk
        {
            public ObsOccCurve[][] obsOccCurves;

            public override void Deserialize(WwiseData wd)
            {
                base.Deserialize(wd);

                obsOccCurves = new ObsOccCurve[2][];
                for (int i = 0; i < obsOccCurves.Length; i++)
                {
                    obsOccCurves[i] = new ObsOccCurve[3];
                    for (int j = 0; j < obsOccCurves[i].Length; j++)
                    {
                        ObsOccCurve ooc = new();
                        ooc.Deserialize(wd);
                        obsOccCurves[i][j] = ooc;
                    }
                }
            }

            public override IEnumerable<byte> Serialize()
            {
                List<byte> b0 = new();
                foreach (ObsOccCurve[] ooca in obsOccCurves)
                    foreach (ObsOccCurve ooc in ooca)
                        b0.AddRange(ooc.Serialize());
                chunkSize = (uint)b0.Count;

                List<byte> b = new();
                b.AddRange(base.Serialize());
                b.AddRange(b0);
                return b;
            }
        }

        public class ObsOccCurve : ISerializable
        {
            public byte curveEnabled;
            public byte curveScaling;
            public ushort curveSize;
            public List<RTPCGraphPoint> points;

            public void Deserialize(WwiseData wd)
            {
                points = new();
                curveEnabled = ReadUInt8(wd);
                curveScaling = ReadUInt8(wd);
                curveSize = ReadUInt16(wd);
                for (int i = 0; i < curveSize; i++)
                {
                    RTPCGraphPoint rtpcgp = new();
                    rtpcgp.Deserialize(wd);
                    points.Add(rtpcgp);
                }
            }

            public IEnumerable<byte> Serialize()
            {
                List<byte> b = new();
                b.Add(curveEnabled);
                b.Add(curveScaling);
                curveSize = (ushort)points.Count;
                b.AddRange(GetBytes(curveSize));
                foreach (RTPCGraphPoint rtpcgp in points)
                    b.AddRange(rtpcgp.Serialize());
                return b;
            }
        }

        public class HircChunk : Chunk
        {
            public uint releasableHircItemCount;
            public List<HircItem> loadedItem;

            public override void Deserialize(WwiseData wd)
            {
                base.Deserialize(wd);

                loadedItem = new();
                releasableHircItemCount = ReadUInt32(wd);
                for (int i = 0; i < releasableHircItemCount; i++)
                    loadedItem.Add(HircItem.Create(wd));
            }

            public override IEnumerable<byte> Serialize()
            {
                SortHircItems();

                List<byte> b0 = new();
                releasableHircItemCount = (uint)loadedItem.Count;
                b0.AddRange(GetBytes(releasableHircItemCount));
                foreach (HircItem hircItem in loadedItem)
                    b0.AddRange(hircItem.Serialize());
                chunkSize = (uint)b0.Count;

                List<byte> b = new();
                b.AddRange(base.Serialize());
                b.AddRange(b0);
                return b;
            }

            private void SortHircItems()
            {
                List<HircItem> newList = new();

                Dictionary<uint, HircItem> hircLookup = new();
                foreach (HircItem hi in loadedItem)
                    hircLookup.Add(hi.id, hi);

                List<HircItem> attenuation = new();
                List<HircItem> media = new();
                List<HircItem> events = new();


                foreach (HircItem hi in loadedItem)
                {
                    if (hi is Attenuation)
                    {
                        attenuation.Add(hi);
                        continue;
                    }
                    if (hi is MusicTrack || hi is Sound || hi is MusicSegment ||
                        hi is MusicRanSeqCntr || hi is MusicSwitchCntr || hi is RanSeqCntr ||
                        hi is ActorMixer || hi is LayerCntr || hi is SwitchCntr)
                    {
                        TryAddMediaItem(hi, media, hircLookup);
                        continue;
                    }
                    if (hi is Event || hi is Action)
                    {
                        TryAddEventItem(hi, events, hircLookup);
                        continue;
                    }
                }

                newList.AddRange(attenuation);
                newList.AddRange(media);
                newList.AddRange(events);

                loadedItem = newList;
            }

            private bool TryAddMediaItem(HircItem hi, List<HircItem> media, Dictionary<uint, HircItem> hircLookup)
            {
                if (media.Contains(hi))
                    return false;

                if (hi is MusicTrack || hi is Sound)
                    DoNothing(); //Whoa! An empty statement!
                else if (hi is MusicSegment ms)
                    foreach (uint childID in ms.musicNodeParams.children.childIDs)
                        TryAddMediaItem(hircLookup[childID], media, hircLookup);
                else if (hi is MusicRanSeqCntr mrsc)
                    foreach (uint childID in mrsc.musicTransNodeParams.musicNodeParams.children.childIDs)
                        TryAddMediaItem(hircLookup[childID], media, hircLookup);
                else if (hi is MusicSwitchCntr msc)
                    foreach (uint childID in msc.musicTransNodeParams.musicNodeParams.children.childIDs)
                        TryAddMediaItem(hircLookup[childID], media, hircLookup);
                else if (hi is RanSeqCntr rsc)
                    foreach (uint childID in rsc.children.childIDs)
                        TryAddMediaItem(hircLookup[childID], media, hircLookup);
                else if (hi is ActorMixer am)
                    foreach (uint childID in am.children.childIDs)
                        TryAddMediaItem(hircLookup[childID], media, hircLookup);
                else if (hi is LayerCntr lc)
                    foreach (uint childID in lc.children.childIDs)
                        TryAddMediaItem(hircLookup[childID], media, hircLookup);
                else if (hi is SwitchCntr sc)
                    foreach (uint childID in sc.children.childIDs)
                        TryAddMediaItem(hircLookup[childID], media, hircLookup);
                else
                    return false;

                media.Add(hi);
                return true;
            }

            private static void DoNothing() { } //Yup, that's right.

            private bool TryAddEventItem(HircItem hi, List<HircItem> events, Dictionary<uint, HircItem> hircLookup)
            {
                if (events.Contains(hi))
                    return false;

                if (hi is Action)
                    events.Add(hi);
                else if (hi is Event e)
                {
                    foreach (uint actionID in e.actionIDs)
                    {
                        HircItem action = hircLookup[actionID];
                        if (!events.Contains(action))
                            events.Add(action);
                    }
                    events.Add(hi);
                }
                else
                    return false;

                return true;
            }
        }

        public class State : HircItem
        {
            public PropBundle4 propBundle4;

            public override void Deserialize(WwiseData wd)
            {
                base.Deserialize(wd);

                propBundle4 = new();
                propBundle4.Deserialize(wd);
            }

            public override IEnumerable<byte> Serialize()
            {
                List<byte> b0 = new();
                b0.AddRange(propBundle4.Serialize());
                sectionSize = (uint)(b0.Count + 4);

                List<byte> b = new();
                b.AddRange(base.Serialize());
                b.AddRange(b0);
                return b;
            }
        }

        public class PropBundle4 : ISerializable
        {
            public ushort propsCount;
            public List<PropBundle5> props;

            public void Deserialize(WwiseData wd)
            {
                props = new();
                propsCount = ReadUInt16(wd);
                for (int i = 0; i < propsCount; i++)
                {
                    PropBundle5 pb1 = new();
                    pb1.Deserialize(wd);
                    props.Add(pb1);
                }
                foreach (PropBundle5 pb1 in props)
                    pb1.DeserializeValue(wd);
            }

            public IEnumerable<byte> Serialize()
            {
                List<byte> b = new();
                propsCount = (byte)props.Count;
                b.AddRange(GetBytes(propsCount));
                foreach (PropBundle5 pb1 in props)
                    b.AddRange(pb1.Serialize());
                foreach (PropBundle5 pb1 in props)
                    b.AddRange(pb1.SerializeValue());
                return b;
            }
        }

        public class PropBundle5 : ISerializable
        {
            public ushort id;
            public float value;

            public void Deserialize(WwiseData wd)
            {
                id = ReadUInt16(wd);
            }

            public void DeserializeValue(WwiseData wd)
            {
                value = ReadSingle(wd);
            }

            public IEnumerable<byte> Serialize()
            {
                return GetBytes(id);
            }

            public IEnumerable<byte> SerializeValue()
            {
                return GetBytes(value);
            }
        }

        public class Sound : HircItem, ICloneable
        {
            public BankSourceData bankSourceData;
            public NodeBaseParams nodeBaseParams;

            public override void Deserialize(WwiseData wd)
            {
                base.Deserialize(wd);

                bankSourceData = new();
                nodeBaseParams = new();
                bankSourceData.Deserialize(wd);
                nodeBaseParams.Deserialize(wd);
            }

            public override IEnumerable<byte> Serialize()
            {
                List<byte> b0 = new();
                b0.AddRange(bankSourceData.Serialize());
                b0.AddRange(nodeBaseParams.Serialize());
                sectionSize = (uint)(b0.Count + 4);

                List<byte> b = new();
                b.AddRange(base.Serialize());
                b.AddRange(b0);
                return b;
            }

            public object Clone()
            {
                Sound s = (Sound)MemberwiseClone();
                s.bankSourceData = (BankSourceData)bankSourceData.Clone();
                s.nodeBaseParams = (NodeBaseParams)nodeBaseParams.Clone();
                return s;
            }
        }

        public class BankSourceData : ISerializable, ICloneable
        {
            public uint pluginID;
            public byte streamType;
            public MediaInformation mediaInformation;

            public void Deserialize(WwiseData wd)
            {
                mediaInformation = new();
                pluginID = ReadUInt32(wd);
                streamType = ReadUInt8(wd);
                mediaInformation.Deserialize(wd);
            }

            public IEnumerable<byte> Serialize()
            {
                List<byte> b = new();
                b.AddRange(GetBytes(pluginID));
                b.Add(streamType);
                b.AddRange(mediaInformation.Serialize());
                return b;
            }

            public object Clone()
            {
                BankSourceData bsd = (BankSourceData)MemberwiseClone();
                bsd.mediaInformation = (MediaInformation)mediaInformation.Clone();
                return bsd;
            }
        }

        public class MediaInformation : ISerializable, ICloneable
        {
            public uint sourceID;
            public uint inMemoryMediaSize;
            public bool isLanguageSpecific;
            public bool prefetch;
            public bool nonCachable;
            public bool hasSource;

            public void Deserialize(WwiseData wd)
            {
                sourceID = ReadUInt32(wd);
                inMemoryMediaSize = ReadUInt32(wd);
                bool[] flags = ReadFlags(wd);
                isLanguageSpecific = flags[0];
                prefetch = flags[1];
                nonCachable = flags[3];
                hasSource = flags[7];
            }

            public IEnumerable<byte> Serialize()
            {
                List<byte> b = new();
                b.AddRange(GetBytes(sourceID));
                b.AddRange(GetBytes(inMemoryMediaSize));
                bool[] flags = {
                    isLanguageSpecific,
                    prefetch,
                    false,
                    nonCachable,
                    false, false, false,
                    hasSource
                };
                b.Add(GetByte(flags));
                return b;
            }

            public object Clone()
            {
                return MemberwiseClone();
            }
        }

        public class NodeBaseParams : ISerializable, ICloneable
        {
            public NodeInitialFxParams nodeInitialFxParams;
            public byte overrideAttachmentParams;
            public uint overrideBusId;
            public uint directParentID;
            public bool priorityOverrideParent;
            public bool priorityApplyDistFactor;
            public bool overrideMidiEventsBehavior;
            public bool overrideMidiNoteTracking;
            public bool enableMidiNoteTracking;
            public bool isMidiBreakLoopOnNoteOff;
            public NodeInitialParams nodeInitialParams;
            public PositioningParams positioningParams;
            public AuxParams auxParams;
            public AdvSettingsParams advSettingsParams;
            public StateChunk stateChunk;
            public InitialRTPC initialRTPC;

            public void Deserialize(WwiseData wd)
            {
                nodeInitialFxParams = new();
                nodeInitialParams = new();
                positioningParams = new();
                auxParams = new();
                advSettingsParams = new();
                stateChunk = new();
                initialRTPC = new();
                nodeInitialFxParams.Deserialize(wd);
                overrideAttachmentParams = ReadUInt8(wd);
                overrideBusId = ReadUInt32(wd);
                directParentID = ReadUInt32(wd);
                bool[] flags = ReadFlags(wd);
                priorityOverrideParent = flags[0];
                priorityApplyDistFactor = flags[1];
                overrideMidiEventsBehavior = flags[2];
                overrideMidiNoteTracking = flags[3];
                enableMidiNoteTracking = flags[4];
                isMidiBreakLoopOnNoteOff = flags[5];
                nodeInitialParams.Deserialize(wd);
                positioningParams.Deserialize(wd);
                auxParams.Deserialize(wd);
                advSettingsParams.Deserialize(wd);
                stateChunk.Deserialize(wd);
                initialRTPC.Deserialize(wd);
            }

            public IEnumerable<byte> Serialize()
            {
                List<byte> b = new();
                b.AddRange(nodeInitialFxParams.Serialize());
                b.Add(overrideAttachmentParams);
                b.AddRange(GetBytes(overrideBusId));
                b.AddRange(GetBytes(directParentID));
                bool[] flags = {
                    priorityOverrideParent,
                    priorityApplyDistFactor,
                    overrideMidiEventsBehavior,
                    overrideMidiNoteTracking,
                    enableMidiNoteTracking,
                    isMidiBreakLoopOnNoteOff
                };
                b.Add(GetByte(flags));
                b.AddRange(nodeInitialParams.Serialize());
                b.AddRange(positioningParams.Serialize());
                b.AddRange(auxParams.Serialize());
                b.AddRange(advSettingsParams.Serialize());
                b.AddRange(stateChunk.Serialize());
                b.AddRange(initialRTPC.Serialize());
                return b;
            }

            public object Clone()
            {
                NodeBaseParams nbp = (NodeBaseParams)MemberwiseClone();
                nbp.nodeInitialFxParams = (NodeInitialFxParams)nodeInitialFxParams.Clone();
                nbp.nodeInitialParams = (NodeInitialParams)nodeInitialParams.Clone();
                nbp.positioningParams = (PositioningParams)positioningParams.Clone();
                nbp.auxParams = (AuxParams)auxParams.Clone();
                nbp.advSettingsParams = (AdvSettingsParams)advSettingsParams.Clone();
                nbp.stateChunk = (StateChunk)stateChunk.Clone();
                nbp.initialRTPC = (InitialRTPC)initialRTPC.Clone();
                return nbp;
            }
        }

        public class NodeInitialFxParams : ISerializable, ICloneable
        {
            public byte isOverrideParentFX;
            public byte numFx;

            public void Deserialize(WwiseData wd)
            {
                isOverrideParentFX = ReadUInt8(wd);
                numFx = ReadUInt8(wd);
            }

            public IEnumerable<byte> Serialize()
            {
                List<byte> b = new();
                b.Add(isOverrideParentFX);
                b.Add(numFx);
                return b;
            }

            public object Clone()
            {
                return MemberwiseClone();
            }
        }

        public class NodeInitialParams : ISerializable, ICloneable
        {
            public PropBundle0 propBundle0;
            public PropBundle2 propBundle2;

            public void Deserialize(WwiseData wd)
            {
                propBundle0 = new();
                propBundle2 = new();
                propBundle0.Deserialize(wd);
                propBundle2.Deserialize(wd);
            }

            public IEnumerable<byte> Serialize()
            {
                List<byte> b = new();
                b.AddRange(propBundle0.Serialize());
                b.AddRange(propBundle2.Serialize());
                return b;
            }

            public object Clone()
            {
                NodeInitialParams nip = new()
                {
                    propBundle0 = (PropBundle0)propBundle0.Clone(),
                    propBundle2 = (PropBundle2)propBundle2.Clone()
                };
                return nip;
            }
        }

        public class PropBundle0 : ISerializable, ICloneable
        {
            public byte propsCount;
            public List<PropBundle1> props;

            public void Deserialize(WwiseData wd)
            {
                props = new();
                propsCount = ReadUInt8(wd);
                for (int i = 0; i < propsCount; i++)
                {
                    PropBundle1 pb1 = new();
                    pb1.Deserialize(wd);
                    props.Add(pb1);
                }
                foreach (PropBundle1 pb1 in props)
                    pb1.DeserializeValue(wd);
            }

            public IEnumerable<byte> Serialize()
            {
                List<byte> b = new();
                propsCount = (byte)props.Count;
                b.Add(propsCount);
                foreach (PropBundle1 pb1 in props)
                    b.AddRange(pb1.Serialize());
                foreach (PropBundle1 pb1 in props)
                    b.AddRange(pb1.SerializeValue());
                return b;
            }

            public object Clone()
            {
                PropBundle0 pb0 = (PropBundle0)MemberwiseClone();
                pb0.props = new();
                foreach (PropBundle1 pb1 in props)
                    pb0.props.Add((PropBundle1)pb1.Clone());
                return pb0;
            }
        }

        public class PropBundle1 : ISerializable, ICloneable
        {
            public byte id;
            public float value;

            public void Deserialize(WwiseData wd)
            {
                id = ReadUInt8(wd);
            }

            public void DeserializeValue(WwiseData wd)
            {
                value = ReadSingle(wd);
            }

            public IEnumerable<byte> Serialize()
            {
                return new byte[] { id };
            }

            public IEnumerable<byte> SerializeValue()
            {
                return GetBytes(value);
            }

            public object Clone()
            {
                return MemberwiseClone();
            }
        }

        public class PropBundle2 : ISerializable, ICloneable
        {
            public byte propsCount;
            public List<PropBundle3> props;

            public void Deserialize(WwiseData wd)
            {
                props = new();
                propsCount = ReadUInt8(wd);
                for (int i = 0; i < propsCount; i++)
                {
                    PropBundle3 pb3 = new();
                    pb3.Deserialize(wd);
                    props.Add(pb3);
                }
                foreach (PropBundle3 pb3 in props)
                    pb3.DeserializeBoundaries(wd);
            }

            public IEnumerable<byte> Serialize()
            {
                List<byte> b = new();
                propsCount = (byte)props.Count;
                b.Add(propsCount);
                foreach (PropBundle3 pb3 in props)
                    b.AddRange(pb3.Serialize());
                foreach (PropBundle3 pb3 in props)
                    b.AddRange(pb3.SerializeBoundaries());
                return b;
            }

            public object Clone()
            {
                PropBundle2 pb2 = (PropBundle2)MemberwiseClone();
                pb2.props = new();
                foreach (PropBundle3 pb3 in props)
                    pb2.props.Add((PropBundle3)pb3.Clone());
                return pb2;
            }
        }

        public class PropBundle3 : ISerializable, ICloneable
        {
            public byte id;
            public byte[] min;
            public byte[] max;

            public void Deserialize(WwiseData wd)
            {
                id = ReadUInt8(wd);
            }

            public void DeserializeBoundaries(WwiseData wd)
            {
                min = ReadUInt8Array(wd, 4);
                max = ReadUInt8Array(wd, 4);
            }

            public IEnumerable<byte> Serialize()
            {
                return new byte[] { id };
            }

            public IEnumerable<byte> SerializeBoundaries()
            {
                List<byte> b = new();
                b.AddRange(min);
                b.AddRange(max);
                return b;
            }

            public object Clone()
            {
                PropBundle3 pb3 = (PropBundle3)MemberwiseClone();
                pb3.min = min.ToArray();
                pb3.max = max.ToArray();
                return pb3;
            }
        }

        public class PositioningParams : ISerializable, ICloneable
        {
            public bool positioningInfoOverrideParent;
            public bool hasListenerRelativeRouting;
            public bool pannerType;
            public bool _3DPositionType;
            public bool spatializationMode;
            public bool unkFlag1;
            public bool enableAttenuation;
            public bool holdEmitterPosAndOrient;
            public bool holdListenerOrient;
            public bool enableDiffraction;
            public bool unkFlag7;

            public void Deserialize(WwiseData wd)
            {
                bool[] flags0 = ReadFlags(wd);
                positioningInfoOverrideParent = flags0[0];
                hasListenerRelativeRouting = flags0[1];
                pannerType = flags0[2];
                _3DPositionType = flags0[5];
                if (hasListenerRelativeRouting)
                {
                    bool[] flags1 = ReadFlags(wd);
                    spatializationMode = flags1[0];
                    unkFlag1 = flags1[1];
                    enableAttenuation = flags1[3];
                    holdEmitterPosAndOrient = flags1[4];
                    holdListenerOrient = flags1[5];
                    enableDiffraction = flags1[6];
                    unkFlag7 = flags1[7];
                }
            }

            public IEnumerable<byte> Serialize()
            {
                List<byte> b = new();
                bool[] flags0 = {
                    positioningInfoOverrideParent,
                    hasListenerRelativeRouting,
                    pannerType,
                    false, false,
                    _3DPositionType
                };
                b.Add(GetByte(flags0));
                if (hasListenerRelativeRouting)
                {
                    bool[] flags1 = {
                        spatializationMode,
                        unkFlag1,
                        false,
                        enableAttenuation,
                        holdEmitterPosAndOrient,
                        holdListenerOrient,
                        enableDiffraction,
                        unkFlag7
                    };
                    b.Add(GetByte(flags1));
                }
                return b;
            }

            public object Clone()
            {
                return MemberwiseClone();
            }
        }

        public class AuxParams : ISerializable, ICloneable
        {
            public bool unkFlag0;
            public bool unkFlag1;
            public bool overrideUserAuxSends;
            public bool hasAux;
            public bool overrideReflectionsAuxBus;
            public uint[] auxIDs;
            public uint reflectionsAuxBus;

            public void Deserialize(WwiseData wd)
            {
                bool[] flags = ReadFlags(wd);
                unkFlag0 = flags[0];
                unkFlag1 = flags[1];
                overrideUserAuxSends = flags[2];
                hasAux = flags[3];
                overrideReflectionsAuxBus = flags[4];
                if (hasAux)
                {
                    auxIDs = new uint[4];
                    for (int i = 0; i < 4; i++)
                        auxIDs[i] = ReadUInt32(wd);
                }
                reflectionsAuxBus = ReadUInt32(wd);
            }

            public IEnumerable<byte> Serialize()
            {
                List<byte> b = new();
                bool[] flags = {
                    unkFlag0,
                    unkFlag1,
                    overrideUserAuxSends,
                    hasAux,
                    overrideReflectionsAuxBus
                };
                b.Add(GetByte(flags));
                if (auxIDs != null)
                    foreach (uint i in auxIDs)
                        b.AddRange(GetBytes(i));
                b.AddRange(GetBytes(reflectionsAuxBus));
                return b;
            }

            public object Clone()
            {
                AuxParams ap = (AuxParams)MemberwiseClone();
                if (auxIDs != null)
                    ap.auxIDs = auxIDs.ToArray();
                return ap;
            }
        }

        public class AdvSettingsParams : ISerializable, ICloneable
        {
            public bool killNewest;
            public bool useVirtualBehavior;
            public bool unkFlag2;
            public bool ignoreParentMaxNumInst;
            public bool isVVoicesOptOverrideParent;
            public byte virtualQueueBehavior;
            public ushort maxNumInstance;
            public byte belowThresholdBehavior;
            public bool overrideHdrEnvelope;
            public bool overrideAnalysis;
            public bool normalizeLoudness;
            public bool enableEnvelope;

            public void Deserialize(WwiseData wd)
            {
                bool[] flags0 = ReadFlags(wd);
                killNewest = flags0[0];
                useVirtualBehavior = flags0[1];
                unkFlag2 = flags0[2];
                ignoreParentMaxNumInst = flags0[3];
                isVVoicesOptOverrideParent = flags0[4];
                virtualQueueBehavior = ReadUInt8(wd);
                maxNumInstance = ReadUInt16(wd);
                belowThresholdBehavior = ReadUInt8(wd);
                bool[] flags1 = ReadFlags(wd);
                overrideHdrEnvelope = flags1[0];
                overrideAnalysis = flags1[1];
                normalizeLoudness = flags1[2];
                enableEnvelope = flags1[3];
            }

            public IEnumerable<byte> Serialize()
            {
                List<byte> b = new();
                bool[] flags0 = {
                    killNewest,
                    useVirtualBehavior,
                    unkFlag2,
                    ignoreParentMaxNumInst,
                    isVVoicesOptOverrideParent
                };
                b.Add(GetByte(flags0));
                b.Add(virtualQueueBehavior);
                b.AddRange(GetBytes(maxNumInstance));
                b.Add(belowThresholdBehavior);
                bool[] flags1 = {
                    overrideHdrEnvelope,
                    overrideAnalysis,
                    normalizeLoudness,
                    enableEnvelope
                };
                b.Add(GetByte(flags1));
                return b;
            }

            public object Clone()
            {
                return MemberwiseClone();
            }
        }

        public class StateChunk : ISerializable, ICloneable
        {
            public ulong statePropsCount;
            public List<StatePropertyInfo> stateProps;
            public ulong stateGroupsCount;
            public List<StateGroupChunk> stateChunks;

            public void Deserialize(WwiseData wd)
            {
                stateProps = new();
                stateChunks = new();
                statePropsCount = ReadVariableInt(wd);
                for (ulong i = 0; i < statePropsCount; i++)
                {
                    StatePropertyInfo spi = new();
                    spi.Deserialize(wd);
                    stateProps.Add(spi);
                }
                stateGroupsCount = ReadVariableInt(wd);
                for (ulong i = 0; i < stateGroupsCount; i++)
                {
                    StateGroupChunk sgc = new();
                    sgc.Deserialize(wd);
                    stateChunks.Add(sgc);
                }
            }

            public IEnumerable<byte> Serialize()
            {
                List<byte> b = new();
                statePropsCount = (ulong)stateProps.Count;
                b.AddRange(GetVariableIntBytes(statePropsCount));
                foreach (StatePropertyInfo spi in stateProps)
                    b.AddRange(spi.Serialize());
                stateGroupsCount = (ulong)stateChunks.Count;
                b.AddRange(GetVariableIntBytes(stateGroupsCount));
                foreach (StateGroupChunk sgc in stateChunks)
                    b.AddRange(sgc.Serialize());
                return b;
            }

            public object Clone()
            {
                StateChunk sc = (StateChunk)MemberwiseClone();
                sc.stateProps = new();
                foreach (StatePropertyInfo spi in stateProps)
                    sc.stateProps.Add((StatePropertyInfo)spi.Clone());
                sc.stateChunks = new();
                foreach (StateGroupChunk sgc in stateChunks)
                    sc.stateChunks.Add((StateGroupChunk)sgc.Clone());
                return sc;
            }
        }

        public class StatePropertyInfo : ISerializable, ICloneable
        {
            public ulong propertyID;
            public byte accumType;
            public byte inDb;

            public void Deserialize(WwiseData wd)
            {
                propertyID = ReadVariableInt(wd);
                accumType = ReadUInt8(wd);
                inDb = ReadUInt8(wd);
            }

            public IEnumerable<byte> Serialize()
            {
                List<byte> b = new();
                b.AddRange(GetVariableIntBytes(propertyID));
                b.Add(accumType);
                b.Add(inDb);
                return b;
            }

            public object Clone()
            {
                return MemberwiseClone();
            }
        }

        public class StateGroupChunk : ISerializable, ICloneable
        {
            public uint stateGroupID;
            public byte stateSyncType;
            public ulong statesCount;
            public List<AkState> states;

            public void Deserialize(WwiseData wd)
            {
                states = new();
                stateGroupID = ReadUInt32(wd);
                stateSyncType = ReadUInt8(wd);
                statesCount = ReadVariableInt(wd);
                for (ulong i = 0; i < statesCount; i++)
                {
                    AkState s = new();
                    s.Deserialize(wd);
                    states.Add(s);
                }
            }

            public IEnumerable<byte> Serialize()
            {
                List<byte> b = new();
                b.AddRange(GetBytes(stateGroupID));
                b.Add(stateSyncType);
                statesCount = (ulong)states.Count;
                b.AddRange(GetVariableIntBytes(statesCount));
                foreach (AkState s in states)
                    b.AddRange(s.Serialize());
                return b;
            }

            public object Clone()
            {
                StateGroupChunk sgc = (StateGroupChunk)MemberwiseClone();
                sgc.states = new();
                foreach (AkState akState in states)
                    sgc.states.Add((AkState)akState.Clone());
                return sgc;
            }
        }

        public class AkState : ISerializable, ICloneable
        {
            public uint stateID;
            public uint stateInstanceID;

            public void Deserialize(WwiseData wd)
            {
                stateID = ReadUInt32(wd);
                stateInstanceID = ReadUInt32(wd);
            }

            public IEnumerable<byte> Serialize()
            {
                List<byte> b = new();
                b.AddRange(GetBytes(stateID));
                b.AddRange(GetBytes(stateInstanceID));
                return b;
            }

            public object Clone()
            {
                return MemberwiseClone();
            }
        }

        public class InitialRTPC : ISerializable, ICloneable
        {
            public ushort rtpcCount;
            public List<RTPC> pRTPCMgr;

            public void Deserialize(WwiseData wd)
            {
                pRTPCMgr = new();
                rtpcCount = ReadUInt16(wd);
                for (int i = 0; i < rtpcCount; i++)
                {
                    RTPC rtpc = new();
                    rtpc.Deserialize(wd);
                    pRTPCMgr.Add(rtpc);
                }
            }

            public IEnumerable<byte> Serialize()
            {
                List<byte> b = new();
                rtpcCount = (ushort)pRTPCMgr.Count;
                b.AddRange(GetBytes(rtpcCount));
                foreach (RTPC rtpc in pRTPCMgr)
                    b.AddRange(rtpc.Serialize());
                return b;
            }

            public object Clone()
            {
                InitialRTPC irtpc = (InitialRTPC)MemberwiseClone();
                irtpc.pRTPCMgr = new();
                foreach (RTPC rtpc in pRTPCMgr)
                    irtpc.pRTPCMgr.Add((RTPC)rtpc.Clone());
                return irtpc;
            }
        }

        public class RTPC : WwiseObject, ICloneable
        {
            public uint rtpcID;
            public byte rtpcType;
            public byte rtpcAccum;
            public ulong paramID;
            public uint rtpcCurveID;
            public byte scaling;
            public ushort size;
            public List<RTPCGraphPoint> pRTPCMgr;

            public override void Deserialize(WwiseData wd)
            {
                pRTPCMgr = new();
                rtpcID = ReadUInt32(wd);
                rtpcType = ReadUInt8(wd);
                rtpcAccum = ReadUInt8(wd);
                paramID = ReadVariableInt(wd);
                rtpcCurveID = ReadUInt32(wd);
                wd.objectsByID[rtpcCurveID] = this;
                scaling = ReadUInt8(wd);
                size = ReadUInt16(wd);
                for (int i = 0; i < size; i++)
                {
                    RTPCGraphPoint rtpcgp = new();
                    rtpcgp.Deserialize(wd);
                    pRTPCMgr.Add(rtpcgp);
                }
            }

            public override IEnumerable<byte> Serialize()
            {
                List<byte> b = new();
                b.AddRange(GetBytes(rtpcID));
                b.Add(rtpcType);
                b.Add(rtpcAccum);
                b.AddRange(GetVariableIntBytes(paramID));
                b.AddRange(GetBytes(rtpcCurveID));
                b.Add(scaling);
                size = (ushort)pRTPCMgr.Count;
                b.AddRange(GetBytes(size));
                foreach (RTPCGraphPoint rtpcgp in pRTPCMgr)
                    b.AddRange(rtpcgp.Serialize());
                return b;
            }

            public object Clone()
            {
                RTPC rtpc = (RTPC)MemberwiseClone();
                rtpc.pRTPCMgr = new();
                foreach (RTPCGraphPoint rtpcgp in pRTPCMgr)
                    rtpc.pRTPCMgr.Add((RTPCGraphPoint)rtpcgp.Clone());
                return rtpc;
            }
        }

        [JsonConverter(typeof(ActionConverter))]
        public abstract class Action : HircItem
        {
            public ushort actionType;
            public uint idExt;
            public byte isBus;
            public PropBundle0 propBundle0;
            public PropBundle2 propBundle1;

            public static Action GetInstance(WwiseData wd)
            {
                ushort actionType = BitConverter.ToUInt16(wd.buffer, wd.offset + 9);
                Action a = actionType switch
                {
                    258 => new ActionStop(),
                    259 => new ActionStop(),
                    260 => new ActionStop(),
                    514 => new ActionPause(),
                    515 => new ActionPause(),
                    516 => new ActionPause(),
                    770 => new ActionResume(),
                    772 => new ActionResume(),
                    1027 => new ActionPlay(),
                    1538 => new ActionMute(),
                    1794 => new ActionMute(),
                    2562 => new ActionSetAkProp(),
                    2818 => new ActionSetAkProp(),
                    3074 => new ActionSetAkProp(),
                    3075 => new ActionSetAkProp(),
                    3330 => new ActionSetAkProp(),
                    3332 => new ActionSetAkProp(),
                    4612 => new ActionSetState(),
                    4866 => new ActionSetGameParameter(),
                    4867 => new ActionSetGameParameter(),
                    5122 => new ActionSetGameParameter(),
                    5123 => new ActionSetGameParameter(),
                    6401 => new ActionSetSwitch(),
                    8451 => new ActionPlayEvent(),
                    _ => throw new NotImplementedException("ActionType " + actionType + " at " + (wd.offset + 9)),
                };
                return a;
            }

            public override void Deserialize(WwiseData wd)
            {
                base.Deserialize(wd);

                propBundle0 = new();
                propBundle1 = new();
                actionType = ReadUInt16(wd);
                idExt = ReadUInt32(wd);
                isBus = ReadUInt8(wd);
                propBundle0.Deserialize(wd);
                propBundle1.Deserialize(wd);
            }

            public override IEnumerable<byte> Serialize()
            {
                List<byte> b = new();
                b.AddRange(base.Serialize());
                b.AddRange(GetBytes(actionType));
                b.AddRange(GetBytes(idExt));
                b.Add(isBus);
                b.AddRange(propBundle0.Serialize());
                b.AddRange(propBundle1.Serialize());
                return b;
            }
        }

        public class ActionSpecifiedConcreteClassConverter : DefaultContractResolver
        {
            protected override JsonConverter ResolveContractConverter(Type objectType)
            {
                if (typeof(Action).IsAssignableFrom(objectType) && !objectType.IsAbstract)
                    return null; // pretend TableSortRuleConvert is not specified (thus avoiding a stack overflow)
                return base.ResolveContractConverter(objectType);
            }
        }

        public class ActionConverter : JsonConverter
        {
            static JsonSerializerSettings SpecifiedSubclassConversion = new JsonSerializerSettings() { ContractResolver = new ActionSpecifiedConcreteClassConverter() };

            public override bool CanConvert(Type objectType)
            {
                return (objectType == typeof(Action));
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                JObject jo = JObject.Load(reader);
                return jo["actionType"].Value<ushort>() switch
                {
                    258 => JsonConvert.DeserializeObject<ActionStop>(jo.ToString(), SpecifiedSubclassConversion),
                    259 => JsonConvert.DeserializeObject<ActionStop>(jo.ToString(), SpecifiedSubclassConversion),
                    260 => JsonConvert.DeserializeObject<ActionStop>(jo.ToString(), SpecifiedSubclassConversion),
                    514 => JsonConvert.DeserializeObject<ActionPause>(jo.ToString(), SpecifiedSubclassConversion),
                    515 => JsonConvert.DeserializeObject<ActionPause>(jo.ToString(), SpecifiedSubclassConversion),
                    516 => JsonConvert.DeserializeObject<ActionPause>(jo.ToString(), SpecifiedSubclassConversion),
                    770 => JsonConvert.DeserializeObject<ActionResume>(jo.ToString(), SpecifiedSubclassConversion),
                    772 => JsonConvert.DeserializeObject<ActionResume>(jo.ToString(), SpecifiedSubclassConversion),
                    1027 => JsonConvert.DeserializeObject<ActionPlay>(jo.ToString(), SpecifiedSubclassConversion),
                    1538 => JsonConvert.DeserializeObject<ActionMute>(jo.ToString(), SpecifiedSubclassConversion),
                    1794 => JsonConvert.DeserializeObject<ActionMute>(jo.ToString(), SpecifiedSubclassConversion),
                    2562 => JsonConvert.DeserializeObject<ActionSetAkProp>(jo.ToString(), SpecifiedSubclassConversion),
                    2818 => JsonConvert.DeserializeObject<ActionSetAkProp>(jo.ToString(), SpecifiedSubclassConversion),
                    3074 => JsonConvert.DeserializeObject<ActionSetAkProp>(jo.ToString(), SpecifiedSubclassConversion),
                    3075 => JsonConvert.DeserializeObject<ActionSetAkProp>(jo.ToString(), SpecifiedSubclassConversion),
                    3330 => JsonConvert.DeserializeObject<ActionSetAkProp>(jo.ToString(), SpecifiedSubclassConversion),
                    3332 => JsonConvert.DeserializeObject<ActionSetAkProp>(jo.ToString(), SpecifiedSubclassConversion),
                    4612 => JsonConvert.DeserializeObject<ActionSetState>(jo.ToString(), SpecifiedSubclassConversion),
                    4866 => JsonConvert.DeserializeObject<ActionSetGameParameter>(jo.ToString(), SpecifiedSubclassConversion),
                    4867 => JsonConvert.DeserializeObject<ActionSetGameParameter>(jo.ToString(), SpecifiedSubclassConversion),
                    5122 => JsonConvert.DeserializeObject<ActionSetGameParameter>(jo.ToString(), SpecifiedSubclassConversion),
                    5123 => JsonConvert.DeserializeObject<ActionSetGameParameter>(jo.ToString(), SpecifiedSubclassConversion),
                    6401 => JsonConvert.DeserializeObject<ActionSetSwitch>(jo.ToString(), SpecifiedSubclassConversion),
                    8451 => JsonConvert.DeserializeObject<ActionPlayEvent>(jo.ToString(), SpecifiedSubclassConversion),
                    _ => throw new NotImplementedException("Invalid actionType: " + jo["actionType"].Value<ushort>()),
                };
            }

            public override bool CanWrite
            {
                get { return false; }
            }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                throw new NotImplementedException(); // won't be called because CanWrite returns false
            }
        }

        public class ActionStop : Action
        {
            public byte fadeCurve;
            public StopActionSpecificParams stopActionSpecificParams;
            public ExceptParams exceptParams;

            public override void Deserialize(WwiseData wd)
            {
                base.Deserialize(wd);

                stopActionSpecificParams = new();
                exceptParams = new();
                fadeCurve = ReadUInt8(wd);
                stopActionSpecificParams.Deserialize(wd);
                exceptParams.Deserialize(wd);
            }

            public override IEnumerable<byte> Serialize()
            {
                List<byte> b = new();
                b.AddRange(base.Serialize());
                b.Add(fadeCurve);
                b.AddRange(stopActionSpecificParams.Serialize());
                b.AddRange(exceptParams.Serialize());
                return b;
            }
        }

        public class StopActionSpecificParams : ISerializable
        {
            public bool applyToStateTransitions;
            public bool applyToDynamicSequence;

            public void Deserialize(WwiseData wd)
            {
                bool[] flags = ReadFlags(wd);
                applyToStateTransitions = flags[1];
                applyToDynamicSequence = flags[2];
            }

            public IEnumerable<byte> Serialize()
            {
                List<byte> b = new();
                bool[] flags = { false,
                        applyToStateTransitions,
                        applyToDynamicSequence
                };
                b.Add(GetByte(flags));
                return b;
            }
        }

        public class ActionPause : Action
        {
            public byte fadeCurve;
            public PauseActionSpecificParams pauseActionSpecificParams;
            public ExceptParams exceptParams;

            public override void Deserialize(WwiseData wd)
            {
                base.Deserialize(wd);

                pauseActionSpecificParams = new();
                exceptParams = new();
                fadeCurve = ReadUInt8(wd);
                pauseActionSpecificParams.Deserialize(wd);
                exceptParams.Deserialize(wd);
            }

            public override IEnumerable<byte> Serialize()
            {
                List<byte> b = new();
                b.AddRange(base.Serialize());
                b.Add(fadeCurve);
                b.AddRange(pauseActionSpecificParams.Serialize());
                b.AddRange(exceptParams.Serialize());
                return b;
            }
        }

        public class PauseActionSpecificParams : ISerializable
        {
            public bool includePendingResume;
            public bool applyToStateTransitions;
            public bool applyToDynamicSequence;

            public void Deserialize(WwiseData wd)
            {
                bool[] flags = ReadFlags(wd);
                includePendingResume = flags[0];
                applyToStateTransitions = flags[1];
                applyToDynamicSequence = flags[2];
            }

            public IEnumerable<byte> Serialize()
            {
                List<byte> b = new();
                bool[] flags = {
                    includePendingResume,
                    applyToStateTransitions,
                    applyToDynamicSequence
                };
                b.Add(GetByte(flags));
                return b;
            }
        }

        public class ActionResume : Action
        {
            public byte fadeCurve;
            public ResumeActionSpecificParams resumeActionSpecificParams;
            public ExceptParams exceptParams;

            public override void Deserialize(WwiseData wd)
            {
                base.Deserialize(wd);

                resumeActionSpecificParams = new();
                exceptParams = new();
                fadeCurve = ReadUInt8(wd);
                resumeActionSpecificParams.Deserialize(wd);
                exceptParams.Deserialize(wd);
            }

            public override IEnumerable<byte> Serialize()
            {
                List<byte> b = new();
                b.AddRange(base.Serialize());
                b.Add(fadeCurve);
                b.AddRange(resumeActionSpecificParams.Serialize());
                b.AddRange(exceptParams.Serialize());
                return b;
            }
        }

        public class ResumeActionSpecificParams : ISerializable
        {
            public bool isMasterResume;
            public bool applyToStateTransitions;
            public bool applyToDynamicSequence;

            public void Deserialize(WwiseData wd)
            {
                bool[] flags = ReadFlags(wd);
                isMasterResume = flags[0];
                applyToStateTransitions = flags[1];
                applyToDynamicSequence = flags[2];
            }

            public IEnumerable<byte> Serialize()
            {
                List<byte> b = new();
                bool[] flags = {
                    isMasterResume,
                    applyToStateTransitions,
                    applyToDynamicSequence
                };
                b.Add(GetByte(flags));
                return b;
            }
        }

        public class ActionPlay : Action, ICloneable
        {
            public byte fadeCurve;
            public uint bankID;

            public override void Deserialize(WwiseData wd)
            {
                base.Deserialize(wd);

                fadeCurve = ReadUInt8(wd);
                bankID = ReadUInt32(wd);
            }

            public override IEnumerable<byte> Serialize()
            {
                List<byte> b = new();
                b.AddRange(base.Serialize());
                b.Add(fadeCurve);
                b.AddRange(GetBytes(bankID));
                return b;
            }

            public object Clone()
            {
                ActionPlay ap = (ActionPlay)MemberwiseClone();
                ap.propBundle0 = (PropBundle0)propBundle0.Clone();
                ap.propBundle1 = (PropBundle2)propBundle1.Clone();
                return ap;
            }
        }

        public class ActionMute : Action
        {
            public byte fadeCurve;
            public ExceptParams exceptParams;

            public override void Deserialize(WwiseData wd)
            {
                base.Deserialize(wd);

                exceptParams = new();
                fadeCurve = ReadUInt8(wd);
                exceptParams.Deserialize(wd);
            }

            public override IEnumerable<byte> Serialize()
            {
                List<byte> b = new();
                b.AddRange(base.Serialize());
                b.Add(fadeCurve);
                b.AddRange(exceptParams.Serialize());
                return b;
            }
        }

        public class ActionSetAkProp : Action
        {
            public byte fadeCurve;
            public AkPropActionSpecificParams akPropActionSpecificParams;
            public ExceptParams exceptParams;

            public override void Deserialize(WwiseData wd)
            {
                base.Deserialize(wd);

                akPropActionSpecificParams = new();
                exceptParams = new();
                fadeCurve = ReadUInt8(wd);
                akPropActionSpecificParams.Deserialize(wd);
                exceptParams.Deserialize(wd);
            }

            public override IEnumerable<byte> Serialize()
            {
                List<byte> b = new();
                b.AddRange(base.Serialize());
                b.Add(fadeCurve);
                b.AddRange(akPropActionSpecificParams.Serialize());
                b.AddRange(exceptParams.Serialize());
                return b;
            }
        }

        public class AkPropActionSpecificParams : ISerializable
        {
            public byte valueMeaning;
            public RandomizerModifier randomizerModifier;

            public void Deserialize(WwiseData wd)
            {
                randomizerModifier = new();
                valueMeaning = ReadUInt8(wd);
                randomizerModifier.Deserialize(wd);
            }

            public IEnumerable<byte> Serialize()
            {
                List<byte> b = new();
                b.Add(valueMeaning);
                b.AddRange(randomizerModifier.Serialize());
                return b;
            }
        }

        public class RandomizerModifier : ISerializable
        {
            public float _base;
            public float min;
            public float max;

            public void Deserialize(WwiseData wd)
            {
                _base = ReadSingle(wd);
                min = ReadSingle(wd);
                max = ReadSingle(wd);
            }

            public IEnumerable<byte> Serialize()
            {
                List<byte> b = new();
                b.AddRange(GetBytes(_base));
                b.AddRange(GetBytes(min));
                b.AddRange(GetBytes(max));
                return b;
            }
        }

        public class ActionSetState : Action
        {
            public uint stateGroupID;
            public uint targetStateID;

            public override void Deserialize(WwiseData wd)
            {
                base.Deserialize(wd);

                stateGroupID = ReadUInt32(wd);
                targetStateID = ReadUInt32(wd);
            }

            public override IEnumerable<byte> Serialize()
            {
                List<byte> b = new();
                b.AddRange(base.Serialize());
                b.AddRange(GetBytes(stateGroupID));
                b.AddRange(GetBytes(targetStateID));
                return b;
            }
        }

        public class ExceptParams : ISerializable
        {
            public ulong exceptionListSize;
            public List<WwiseObjectIDext> listElementException;

            public void Deserialize(WwiseData wd)
            {
                listElementException = new();
                exceptionListSize = ReadVariableInt(wd);
                for (ulong i = 0; i < exceptionListSize; i++)
                {
                    WwiseObjectIDext woid = new();
                    woid.Deserialize(wd);
                    listElementException.Add(woid);
                }
            }

            public IEnumerable<byte> Serialize()
            {
                List<byte> b = new();
                exceptionListSize = (ulong)listElementException.Count;
                b.AddRange(GetVariableIntBytes(exceptionListSize));
                foreach (WwiseObjectIDext woid in listElementException)
                    b.AddRange(woid.Serialize());
                return b;
            }
        }

        public class WwiseObjectIDext : ISerializable
        {
            public uint id;
            public byte isBus;

            public void Deserialize(WwiseData wd)
            {
                id = ReadUInt32(wd);
                isBus = ReadUInt8(wd);
            }

            public IEnumerable<byte> Serialize()
            {
                List<byte> b = new();
                b.AddRange(GetBytes(id));
                b.Add(isBus);
                return b;
            }
        }

        public class ActionSetGameParameter : Action
        {
            public byte fadeCurve;
            public GameParameterActionSpecificParams gameParameterActionSpecificParams;
            public ExceptParams exceptParams;

            public override void Deserialize(WwiseData wd)
            {
                base.Deserialize(wd);

                gameParameterActionSpecificParams = new();
                exceptParams = new();
                fadeCurve = ReadUInt8(wd);
                gameParameterActionSpecificParams.Deserialize(wd);
                exceptParams.Deserialize(wd);
            }

            public override IEnumerable<byte> Serialize()
            {
                List<byte> b = new();
                b.AddRange(base.Serialize());
                b.Add(fadeCurve);
                b.AddRange(gameParameterActionSpecificParams.Serialize());
                b.AddRange(exceptParams.Serialize());
                return b;
            }
        }

        public class GameParameterActionSpecificParams : ISerializable
        {
            public byte bypassTransition;
            public byte valueMeaning;
            public RangedParameter rangedParameter;

            public void Deserialize(WwiseData wd)
            {
                rangedParameter = new();
                bypassTransition = ReadUInt8(wd);
                valueMeaning = ReadUInt8(wd);
                rangedParameter.Deserialize(wd);
            }

            public IEnumerable<byte> Serialize()
            {
                List<byte> b = new();
                b.Add(bypassTransition);
                b.Add(valueMeaning);
                b.AddRange(rangedParameter.Serialize());
                return b;
            }
        }

        public class RangedParameter : ISerializable
        {
            public float _base;
            public float min;
            public float max;

            public void Deserialize(WwiseData wd)
            {
                _base = ReadSingle(wd);
                min = ReadSingle(wd);
                max = ReadSingle(wd);
            }

            public IEnumerable<byte> Serialize()
            {
                List<byte> b = new();
                b.AddRange(GetBytes(_base));
                b.AddRange(GetBytes(min));
                b.AddRange(GetBytes(max));
                return b;
            }
        }

        public class ActionSetSwitch : Action
        {
            public uint switchGroupID;
            public uint switchStateID;

            public override void Deserialize(WwiseData wd)
            {
                base.Deserialize(wd);

                switchGroupID = ReadUInt32(wd);
                switchStateID = ReadUInt32(wd);
            }

            public override IEnumerable<byte> Serialize()
            {
                List<byte> b = new();
                b.AddRange(base.Serialize());
                b.AddRange(GetBytes(switchGroupID));
                b.AddRange(GetBytes(switchStateID));
                return b;
            }
        }

        public class ActionPlayEvent : Action { }

        public class Event : HircItem, ICloneable
        {
            public ulong actionListSize;
            public List<uint> actionIDs;

            public override void Deserialize(WwiseData wd)
            {
                base.Deserialize(wd);

                actionIDs = new();
                actionListSize = ReadVariableInt(wd);
                for (ulong i = 0; i < actionListSize; i++)
                    actionIDs.Add(ReadUInt32(wd));
            }

            public override IEnumerable<byte> Serialize()
            {
                List<byte> b0 = new();
                actionListSize = (ulong)actionIDs.Count;
                b0.AddRange(GetVariableIntBytes(actionListSize));
                foreach (uint i in actionIDs)
                    b0.AddRange(GetBytes(i));
                sectionSize = (uint)(b0.Count + 4);

                List<byte> b = new();
                b.AddRange(base.Serialize());
                b.AddRange(b0);
                return b;
            }

            public object Clone()
            {
                Event e = (Event)MemberwiseClone();
                e.actionIDs = new();
                e.actionIDs.AddRange(actionIDs);
                return e;
            }
        }

        public class RanSeqCntr : HircItem
        {
            public NodeBaseParams nodeBaseParams;
            public ushort loopCount;
            public ushort loopModMin;
            public ushort loopModMax;
            public float transitionTime;
            public float transitionTimeModMin;
            public float transitionTimeModMax;
            public ushort avoidRepeatCount;
            public byte transitionMode;
            public byte randomMode;
            public byte mode;
            public bool isUsingWeight;
            public bool resetPlayListAtEachPlay;
            public bool isRestartBackward;
            public bool isContinuous;
            public bool isGlobal;
            public Children children;
            public PlayList playList;

            public override void Deserialize(WwiseData wd)
            {
                base.Deserialize(wd);

                nodeBaseParams = new();
                children = new();
                playList = new();
                nodeBaseParams.Deserialize(wd);
                loopCount = ReadUInt16(wd);
                loopModMin = ReadUInt16(wd);
                loopModMax = ReadUInt16(wd);
                transitionTime = ReadSingle(wd);
                transitionTimeModMin = ReadSingle(wd);
                transitionTimeModMax = ReadSingle(wd);
                avoidRepeatCount = ReadUInt16(wd);
                transitionMode = ReadUInt8(wd);
                randomMode = ReadUInt8(wd);
                mode = ReadUInt8(wd);
                bool[] flags = ReadFlags(wd);
                isUsingWeight = flags[0];
                resetPlayListAtEachPlay = flags[1];
                isRestartBackward = flags[2];
                isContinuous = flags[3];
                isGlobal = flags[4];
                children.Deserialize(wd);
                playList.Deserialize(wd);
            }

            public override IEnumerable<byte> Serialize()
            {
                List<byte> b0 = new();
                b0.AddRange(nodeBaseParams.Serialize());
                b0.AddRange(GetBytes(loopCount));
                b0.AddRange(GetBytes(loopModMin));
                b0.AddRange(GetBytes(loopModMax));
                b0.AddRange(GetBytes(transitionTime));
                b0.AddRange(GetBytes(transitionTimeModMin));
                b0.AddRange(GetBytes(transitionTimeModMax));
                b0.AddRange(GetBytes(avoidRepeatCount));
                b0.Add(transitionMode);
                b0.Add(randomMode);
                b0.Add(mode);
                bool[] flags = {
                    isUsingWeight,
                    resetPlayListAtEachPlay,
                    isRestartBackward,
                    isContinuous,
                    isGlobal
                };
                b0.Add(GetByte(flags));
                b0.AddRange(children.Serialize());
                b0.AddRange(playList.Serialize());
                sectionSize = (uint)(b0.Count + 4);

                List<byte> b = new();
                b.AddRange(base.Serialize());
                b.AddRange(b0);
                return b;
            }
        }

        public class PlayList : ISerializable
        {
            public ushort playListItem;
            public List<PlaylistItem> items;

            public void Deserialize(WwiseData wd)
            {
                items = new();
                playListItem = ReadUInt16(wd);
                for (int i = 0; i < playListItem; i++)
                {
                    PlaylistItem pli = new();
                    pli.Deserialize(wd);
                    items.Add(pli);
                }
            }

            public IEnumerable<byte> Serialize()
            {
                List<byte> b = new();
                playListItem = (ushort)items.Count;
                b.AddRange(GetBytes(playListItem));
                foreach (PlaylistItem pli in items)
                    b.AddRange(pli.Serialize());
                return b;
            }
        }

        public class PlaylistItem : ISerializable
        {
            public uint playID;
            public int weight;

            public void Deserialize(WwiseData wd)
            {
                playID = ReadUInt32(wd);
                weight = ReadInt32(wd);
            }

            public IEnumerable<byte> Serialize()
            {
                List<byte> b = new();
                b.AddRange(GetBytes(playID));
                b.AddRange(GetBytes(weight));
                return b;
            }
        }

        public class SwitchCntr : HircItem
        {
            public NodeBaseParams nodeBaseParams;
            public byte groupType;
            public uint groupID;
            public uint defaultSwitch;
            public byte isContinuousValidation;
            public Children children;
            public uint switchGroupsCount;
            public List<SwitchPackage> switchList;
            public uint switchParamsCount;
            public List<SwitchNodeParams> paramList;

            public override void Deserialize(WwiseData wd)
            {
                base.Deserialize(wd);

                nodeBaseParams = new();
                children = new();
                switchList = new();
                paramList = new();
                nodeBaseParams.Deserialize(wd);
                groupType = ReadUInt8(wd);
                groupID = ReadUInt32(wd);
                defaultSwitch = ReadUInt32(wd);
                isContinuousValidation = ReadUInt8(wd);
                children.Deserialize(wd);
                switchGroupsCount = ReadUInt32(wd);
                for (int i = 0; i < switchGroupsCount; i++)
                {
                    SwitchPackage sp = new();
                    sp.Deserialize(wd);
                    switchList.Add(sp);
                }
                switchParamsCount = ReadUInt32(wd);
                for (int i = 0; i < switchParamsCount; i++)
                {
                    SwitchNodeParams snp = new();
                    snp.Deserialize(wd);
                    paramList.Add(snp);
                }
            }

            public override IEnumerable<byte> Serialize()
            {
                List<byte> b0 = new();
                b0.AddRange(nodeBaseParams.Serialize());
                b0.Add(groupType);
                b0.AddRange(GetBytes(groupID));
                b0.AddRange(GetBytes(defaultSwitch));
                b0.Add(isContinuousValidation);
                b0.AddRange(children.Serialize());
                switchGroupsCount = (uint)switchList.Count;
                b0.AddRange(GetBytes(switchGroupsCount));
                foreach (SwitchPackage sp in switchList)
                    b0.AddRange(sp.Serialize());
                switchParamsCount = (uint)paramList.Count;
                b0.AddRange(GetBytes(switchParamsCount));
                foreach (SwitchNodeParams snp in paramList)
                    b0.AddRange(snp.Serialize());
                sectionSize = (uint)(b0.Count + 4);

                List<byte> b = new();
                b.AddRange(base.Serialize());
                b.AddRange(b0);
                return b;
            }
        }

        public class SwitchPackage : WwiseObject
        {
            public uint switchID;
            public uint itemsCount;
            public List<uint> nodeIDs;

            public override void Deserialize(WwiseData wd)
            {
                nodeIDs = new();
                switchID = ReadUInt32(wd);
                wd.objectsByID[switchID] = this;
                itemsCount = ReadUInt32(wd);
                for (int i = 0; i < itemsCount; i++)
                    nodeIDs.Add(ReadUInt32(wd));
            }

            public override IEnumerable<byte> Serialize()
            {
                List<byte> b = new();
                b.AddRange(GetBytes(switchID));
                itemsCount = (uint)nodeIDs.Count;
                b.AddRange(GetBytes(itemsCount));
                foreach (uint i in nodeIDs)
                    b.AddRange(GetBytes(i));
                return b;
            }
        }

        public class SwitchNodeParams : ISerializable
        {
            public uint nodeID;
            public bool isFirstOnly;
            public bool continuePlayback;
            public byte onSwitchMode;
            public int fadeOutTime;
            public int fadeInTime;

            public void Deserialize(WwiseData wd)
            {
                nodeID = ReadUInt32(wd);
                bool[] flags = ReadFlags(wd);
                isFirstOnly = flags[0];
                continuePlayback = flags[1];
                onSwitchMode = ReadUInt8(wd);
                fadeOutTime = ReadInt32(wd);
                fadeInTime = ReadInt32(wd);
            }

            public IEnumerable<byte> Serialize()
            {
                List<byte> b = new();
                b.AddRange(GetBytes(nodeID));
                bool[] flags = {
                    isFirstOnly,
                    continuePlayback
                };
                b.Add(GetByte(flags));
                b.Add(onSwitchMode);
                b.AddRange(GetBytes(fadeOutTime));
                b.AddRange(GetBytes(fadeInTime));
                return b;
            }
        }

        public class ActorMixer : HircItem, ICloneable
        {
            public NodeBaseParams nodeBaseParams;
            public Children children;

            public override void Deserialize(WwiseData wd)
            {
                base.Deserialize(wd);

                nodeBaseParams = new();
                children = new();
                nodeBaseParams.Deserialize(wd);
                children.Deserialize(wd);
            }

            public override IEnumerable<byte> Serialize()
            {
                List<byte> b0 = new();
                b0.AddRange(nodeBaseParams.Serialize());
                b0.AddRange(children.Serialize());
                sectionSize = (uint)(b0.Count + 4);

                List<byte> b = new();
                b.AddRange(base.Serialize());
                b.AddRange(b0);
                return b;
            }

            public object Clone()
            {
                ActorMixer am = (ActorMixer)MemberwiseClone();
                am.nodeBaseParams = (NodeBaseParams)nodeBaseParams.Clone();
                am.children = (Children)children.Clone();
                return am;
            }
        }

        public class Children : ISerializable, ICloneable
        {
            public uint childsCount;
            public List<uint> childIDs;

            public void Deserialize(WwiseData wd)
            {
                childIDs = new();
                childsCount = ReadUInt32(wd);
                for (int i = 0; i < childsCount; i++)
                    childIDs.Add(ReadUInt32(wd));
            }

            public IEnumerable<byte> Serialize()
            {
                childIDs.Sort();

                List<byte> b = new();
                childsCount = (uint)childIDs.Count;
                b.AddRange(GetBytes(childsCount));
                foreach (uint i in childIDs)
                    b.AddRange(GetBytes(i));
                return b;
            }

            public object Clone()
            {
                Children c = (Children)MemberwiseClone();
                c.childIDs = new();
                c.childIDs.AddRange(childIDs);
                return c;
            }
        }

        public class FxCustom : HircItem
        {
            public FXParams fxParams;
            public byte bankDataCount;
            public List<Unk> media;
            public InitialRTPC initialRTPC;
            public StateChunk stateChunk;
            public ushort valuesCount;
            public List<PluginPropertyValue> propertyValues;

            public override void Deserialize(WwiseData wd)
            {
                base.Deserialize(wd);

                media = new();
                initialRTPC = new();
                stateChunk = new();
                propertyValues = new();
                fxParams = FXParams.Create(wd);
                bankDataCount = ReadUInt8(wd);
                for (int i = 0; i < bankDataCount; i++)
                {
                    Unk u = new();
                    u.Deserialize(wd);
                    media.Add(u);
                }
                initialRTPC.Deserialize(wd);
                stateChunk.Deserialize(wd);
                valuesCount = ReadUInt16(wd);
                for (int i = 0; i < valuesCount; i++)
                {
                    PluginPropertyValue ppv = new();
                    ppv.Deserialize(wd);
                    propertyValues.Add(ppv);
                }
            }

            public override IEnumerable<byte> Serialize()
            {
                List<byte> b0 = new();
                b0.AddRange(GetBytes(fxParams.fxID));
                b0.AddRange(GetBytes(fxParams.size));
                b0.AddRange(fxParams.Serialize());
                bankDataCount = (byte)media.Count;
                b0.Add(bankDataCount);
                foreach (Unk u in media)
                    b0.AddRange(u.Serialize());
                b0.AddRange(initialRTPC.Serialize());
                b0.AddRange(stateChunk.Serialize());
                valuesCount = (ushort)propertyValues.Count;
                b0.AddRange(GetBytes(valuesCount));
                foreach (PluginPropertyValue ppv in propertyValues)
                    b0.AddRange(ppv.Serialize());
                sectionSize = (uint)(b0.Count + 4);

                List<byte> b = new();
                b.AddRange(base.Serialize());
                b.AddRange(b0);
                return b;
            }
        }

        [JsonConverter(typeof(FXParamsConverter))]
        public abstract class FXParams : ISerializable
        {
            public uint fxID;
            public uint size;
            public abstract void Deserialize(WwiseData wd);

            public static FXParams Create(WwiseData wd)
            {
                uint fxID = ReadUInt32(wd);
                FXParams fxp = fxID switch
                {
                    6881283 => new ParameterEQFXParams(),
                    7733251 => new StereoDelayFXParams(),
                    8192003 => new FlangerFXParams(),
                    8454147 => new MeterFXParams(),
                    _ => throw new NotImplementedException("fxID " + fxID + " at " + wd.offset),
                };
                fxp.fxID = fxID;
                fxp.size = ReadUInt32(wd);
                fxp.Deserialize(wd);
                return fxp;
            }

            public abstract IEnumerable<byte> Serialize();
        }

        public class FXParamsSpecifiedConcreteClassConverter : DefaultContractResolver
        {
            protected override JsonConverter ResolveContractConverter(Type objectType)
            {
                if (typeof(FXParams).IsAssignableFrom(objectType) && !objectType.IsAbstract)
                    return null; // pretend TableSortRuleConvert is not specified (thus avoiding a stack overflow)
                return base.ResolveContractConverter(objectType);
            }
        }

        public class FXParamsConverter : JsonConverter
        {
            static JsonSerializerSettings SpecifiedSubclassConversion = new JsonSerializerSettings() { ContractResolver = new FXParamsSpecifiedConcreteClassConverter() };

            public override bool CanConvert(Type objectType)
            {
                return (objectType == typeof(FXParams));
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                JObject jo = JObject.Load(reader);
                return jo["fxID"].Value<uint>() switch
                {
                    6881283 => JsonConvert.DeserializeObject<ParameterEQFXParams>(jo.ToString(), SpecifiedSubclassConversion),
                    7733251 => JsonConvert.DeserializeObject<StereoDelayFXParams>(jo.ToString(), SpecifiedSubclassConversion),
                    8192003 => JsonConvert.DeserializeObject<FlangerFXParams>(jo.ToString(), SpecifiedSubclassConversion),
                    8454147 => JsonConvert.DeserializeObject<MeterFXParams>(jo.ToString(), SpecifiedSubclassConversion),
                    _ => throw new NotImplementedException("Invalid fxID: " + jo["fxID"].Value<uint>()),
                };
            }

            public override bool CanWrite
            {
                get { return false; }
            }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                throw new NotImplementedException(); // won't be called because CanWrite returns false
            }
        }

        public class ParameterEQFXParams : FXParams
        {
            public List<EQModuleParams> band;
            public float outputLevel;
            public byte processLFE;

            public override void Deserialize(WwiseData wd)
            {
                band = new();
                for (int i = 0; i < 3; i++)
                {
                    EQModuleParams eqmp = new();
                    eqmp.Deserialize(wd);
                    band.Add(eqmp);
                }
                outputLevel = ReadSingle(wd);
                processLFE = ReadUInt8(wd);
            }

            public override IEnumerable<byte> Serialize()
            {
                List<byte> b = new();
                foreach (EQModuleParams eqmp in band)
                    b.AddRange(eqmp.Serialize());
                b.AddRange(GetBytes(outputLevel));
                b.Add(processLFE);
                return b;
            }
        }

        public class EQModuleParams : ISerializable
        {
            public uint filterType;
            public float gain;
            public float frequency;
            public float qFactor;
            public byte onOff;

            public void Deserialize(WwiseData wd)
            {
                filterType = ReadUInt32(wd);
                gain = ReadSingle(wd);
                frequency = ReadSingle(wd);
                qFactor = ReadSingle(wd);
                onOff = ReadUInt8(wd);
            }

            public IEnumerable<byte> Serialize()
            {
                List<byte> b = new();
                b.AddRange(GetBytes(filterType));
                b.AddRange(GetBytes(gain));
                b.AddRange(GetBytes(frequency));
                b.AddRange(GetBytes(qFactor));
                b.Add(onOff);
                return b;
            }
        }

        public class StereoDelayFXParams : FXParams
        {
            public RTPCParams rtpcParams;
            public InvariantParams invariantParams;
            public AlgorithmTunings algorithmTunings;

            public override void Deserialize(WwiseData wd)
            {
                rtpcParams = new();
                invariantParams = new();
                algorithmTunings = new();
                rtpcParams.Deserialize(wd);
                invariantParams.Deserialize(wd);
                algorithmTunings.Deserialize(wd);
            }

            public override IEnumerable<byte> Serialize()
            {
                List<byte> b = new();
                b.AddRange(rtpcParams.Serialize());
                b.AddRange(invariantParams.Serialize());
                b.AddRange(algorithmTunings.Serialize());
                return b;
            }
        }

        public class RTPCParams : ISerializable
        {
            public float decayTime;
            public float hfDamping;
            public float diffusion;
            public float stereoWidth;
            public float filter1Gain;
            public float filter1Freq;
            public float filter1Q;
            public float filter2Gain;
            public float filter2Freq;
            public float filter2Q;
            public float filter3Gain;
            public float filter3Freq;
            public float filter3Q;
            public float frontLevel;
            public float rearLevel;
            public float centerLevel;
            public float lfeLevel;
            public float dryLevel;
            public float erLevel;
            public float reverbLevel;

            public void Deserialize(WwiseData wd)
            {
                decayTime = ReadSingle(wd);
                hfDamping = ReadSingle(wd);
                diffusion = ReadSingle(wd);
                stereoWidth = ReadSingle(wd);
                filter1Gain = ReadSingle(wd);
                filter1Freq = ReadSingle(wd);
                filter1Q = ReadSingle(wd);
                filter2Gain = ReadSingle(wd);
                filter2Freq = ReadSingle(wd);
                filter2Q = ReadSingle(wd);
                filter3Gain = ReadSingle(wd);
                filter3Freq = ReadSingle(wd);
                filter3Q = ReadSingle(wd);
                frontLevel = ReadSingle(wd);
                rearLevel = ReadSingle(wd);
                centerLevel = ReadSingle(wd);
                lfeLevel = ReadSingle(wd);
                dryLevel = ReadSingle(wd);
                erLevel = ReadSingle(wd);
                reverbLevel = ReadSingle(wd);
            }

            public IEnumerable<byte> Serialize()
            {
                List<byte> b = new();
                b.AddRange(GetBytes(decayTime));
                b.AddRange(GetBytes(hfDamping));
                b.AddRange(GetBytes(diffusion));
                b.AddRange(GetBytes(stereoWidth));
                b.AddRange(GetBytes(filter1Gain));
                b.AddRange(GetBytes(filter1Freq));
                b.AddRange(GetBytes(filter1Q));
                b.AddRange(GetBytes(filter2Gain));
                b.AddRange(GetBytes(filter2Freq));
                b.AddRange(GetBytes(filter2Q));
                b.AddRange(GetBytes(filter3Gain));
                b.AddRange(GetBytes(filter3Freq));
                b.AddRange(GetBytes(filter3Q));
                b.AddRange(GetBytes(frontLevel));
                b.AddRange(GetBytes(rearLevel));
                b.AddRange(GetBytes(centerLevel));
                b.AddRange(GetBytes(lfeLevel));
                b.AddRange(GetBytes(dryLevel));
                b.AddRange(GetBytes(erLevel));
                b.AddRange(GetBytes(reverbLevel));
                return b;
            }
        }

        public class InvariantParams : ISerializable
        {
            public byte enableEarlyReflections;
            public uint erPattern;
            public float reverbDelay;
            public float roomSize;
            public float erFrontBackDelay;
            public float density;
            public float roomShape;
            public uint numReverbUnits;
            public byte enableToneControls;
            public uint filter1Pos;
            public uint filter1Curve;
            public uint filter2Pos;
            public uint filter2Curve;
            public uint filter3Pos;
            public uint filter3Curve;
            public float inputCenterLevel;
            public float inputLFELevel;

            public void Deserialize(WwiseData wd)
            {
                enableEarlyReflections = ReadUInt8(wd);
                erPattern = ReadUInt32(wd);
                reverbDelay = ReadSingle(wd);
                roomSize = ReadSingle(wd);
                erFrontBackDelay = ReadSingle(wd);
                density = ReadSingle(wd);
                roomShape = ReadSingle(wd);
                numReverbUnits = ReadUInt32(wd);
                enableToneControls = ReadUInt8(wd);
                filter1Pos = ReadUInt32(wd);
                filter1Curve = ReadUInt32(wd);
                filter2Pos = ReadUInt32(wd);
                filter2Curve = ReadUInt32(wd);
                filter3Pos = ReadUInt32(wd);
                filter3Curve = ReadUInt32(wd);
                inputCenterLevel = ReadSingle(wd);
                inputLFELevel = ReadSingle(wd);
            }

            public IEnumerable<byte> Serialize()
            {
                List<byte> b = new();
                b.Add(enableEarlyReflections);
                b.AddRange(GetBytes(erPattern));
                b.AddRange(GetBytes(reverbDelay));
                b.AddRange(GetBytes(roomSize));
                b.AddRange(GetBytes(erFrontBackDelay));
                b.AddRange(GetBytes(density));
                b.AddRange(GetBytes(roomShape));
                b.AddRange(GetBytes(numReverbUnits));
                b.Add(enableToneControls);
                b.AddRange(GetBytes(filter1Pos));
                b.AddRange(GetBytes(filter1Curve));
                b.AddRange(GetBytes(filter2Pos));
                b.AddRange(GetBytes(filter2Curve));
                b.AddRange(GetBytes(filter3Pos));
                b.AddRange(GetBytes(filter3Curve));
                b.AddRange(GetBytes(inputCenterLevel));
                b.AddRange(GetBytes(inputLFELevel));
                return b;
            }
        }

        public class AlgorithmTunings : ISerializable
        {
            public float densityDelayMin;
            public float densityDelayMax;
            public float densityDelayRdmPerc;
            public float roomShapeMin;
            public float roomShapeMax;
            public float diffusionDelayScalePerc;
            public float diffusionDelayMax;
            public float diffusionDelayRdmPerc;
            public float dcFilterCutFreq;
            public float reverbUnitInputDelay;
            public float reverbUnitInputDelayRmdPerc;

            public void Deserialize(WwiseData wd)
            {
                densityDelayMin = ReadSingle(wd);
                densityDelayMax = ReadSingle(wd);
                densityDelayRdmPerc = ReadSingle(wd);
                roomShapeMin = ReadSingle(wd);
                roomShapeMax = ReadSingle(wd);
                diffusionDelayScalePerc = ReadSingle(wd);
                diffusionDelayMax = ReadSingle(wd);
                diffusionDelayRdmPerc = ReadSingle(wd);
                dcFilterCutFreq = ReadSingle(wd);
                reverbUnitInputDelay = ReadSingle(wd);
                reverbUnitInputDelayRmdPerc = ReadSingle(wd);
            }

            public IEnumerable<byte> Serialize()
            {
                List<byte> b = new();
                b.AddRange(GetBytes(densityDelayMin));
                b.AddRange(GetBytes(densityDelayMax));
                b.AddRange(GetBytes(densityDelayRdmPerc));
                b.AddRange(GetBytes(roomShapeMin));
                b.AddRange(GetBytes(roomShapeMax));
                b.AddRange(GetBytes(diffusionDelayScalePerc));
                b.AddRange(GetBytes(diffusionDelayMax));
                b.AddRange(GetBytes(diffusionDelayRdmPerc));
                b.AddRange(GetBytes(dcFilterCutFreq));
                b.AddRange(GetBytes(reverbUnitInputDelay));
                b.AddRange(GetBytes(reverbUnitInputDelayRmdPerc));
                return b;
            }
        }

        public class FlangerFXParams : FXParams
        {
            public float nonRTPCDelayTime;
            public float RTPCDryLevel;
            public float RTPCFfwdLevel;
            public float RTPCFbackLevel;
            public float RTPCModDepth;
            public float RTPCModParamsLFOParamsFrequency;
            public uint RTPCModParamsLFOParamsWaveform;
            public float RTPCModParamsLFOParamsSmooth;
            public float RTPCModParamsLFOParamsPWM;
            public float RTPCModParamsPhaseParamsPhaseOffset;
            public uint RTPCModParamsPhaseParamsPhaseMode;
            public float RTPCModParamsPhaseParamsPhaseSpread;
            public float RTPCOutputLevel;
            public float RTPCWetDryMix;
            public byte nonRTPCEnableLFO;
            public byte nonRTPCProcessCenter;
            public byte nonRTPCProcessLFE;

            public override void Deserialize(WwiseData wd)
            {
                nonRTPCDelayTime = ReadSingle(wd);
                RTPCDryLevel = ReadSingle(wd);
                RTPCFfwdLevel = ReadSingle(wd);
                RTPCFbackLevel = ReadSingle(wd);
                RTPCModDepth = ReadSingle(wd);
                RTPCModParamsLFOParamsFrequency = ReadSingle(wd);
                RTPCModParamsLFOParamsWaveform = ReadUInt32(wd);
                RTPCModParamsLFOParamsSmooth = ReadSingle(wd);
                RTPCModParamsLFOParamsPWM = ReadSingle(wd);
                RTPCModParamsPhaseParamsPhaseOffset = ReadSingle(wd);
                RTPCModParamsPhaseParamsPhaseMode = ReadUInt32(wd);
                RTPCModParamsPhaseParamsPhaseSpread = ReadSingle(wd);
                RTPCOutputLevel = ReadSingle(wd);
                RTPCWetDryMix = ReadSingle(wd);
                nonRTPCEnableLFO = ReadUInt8(wd);
                nonRTPCProcessCenter = ReadUInt8(wd);
                nonRTPCProcessLFE = ReadUInt8(wd);
            }

            public override IEnumerable<byte> Serialize()
            {
                List<byte> b = new();
                b.AddRange(GetBytes(nonRTPCDelayTime));
                b.AddRange(GetBytes(RTPCDryLevel));
                b.AddRange(GetBytes(RTPCFfwdLevel));
                b.AddRange(GetBytes(RTPCFbackLevel));
                b.AddRange(GetBytes(RTPCModDepth));
                b.AddRange(GetBytes(RTPCModParamsLFOParamsFrequency));
                b.AddRange(GetBytes(RTPCModParamsLFOParamsWaveform));
                b.AddRange(GetBytes(RTPCModParamsLFOParamsSmooth));
                b.AddRange(GetBytes(RTPCModParamsLFOParamsPWM));
                b.AddRange(GetBytes(RTPCModParamsPhaseParamsPhaseOffset));
                b.AddRange(GetBytes(RTPCModParamsPhaseParamsPhaseMode));
                b.AddRange(GetBytes(RTPCModParamsPhaseParamsPhaseSpread));
                b.AddRange(GetBytes(RTPCOutputLevel));
                b.AddRange(GetBytes(RTPCWetDryMix));
                b.Add(nonRTPCEnableLFO);
                b.Add(nonRTPCProcessCenter);
                b.Add(nonRTPCProcessLFE);
                return b;
            }
        }

        public class MeterFXParams : FXParams
        {
            public float rtpcAttack;
            public float rtpcRelease;
            public float rtpcMin;
            public float rtpcMax;
            public float rtpcHold;
            public byte nonRTPCMode;
            public byte nonRTPCScope;
            public byte nonRTPCApplyDownstreamVolume;
            public uint nonRTPCGameParamID;

            public override void Deserialize(WwiseData wd)
            {
                rtpcAttack = ReadSingle(wd);
                rtpcRelease = ReadSingle(wd);
                rtpcMin = ReadSingle(wd);
                rtpcMax = ReadSingle(wd);
                rtpcHold = ReadSingle(wd);
                nonRTPCMode = ReadUInt8(wd);
                nonRTPCScope = ReadUInt8(wd);
                nonRTPCApplyDownstreamVolume = ReadUInt8(wd);
                nonRTPCGameParamID = ReadUInt32(wd);
            }

            public override IEnumerable<byte> Serialize()
            {
                List<byte> b = new();
                b.AddRange(GetBytes(rtpcAttack));
                b.AddRange(GetBytes(rtpcRelease));
                b.AddRange(GetBytes(rtpcMin));
                b.AddRange(GetBytes(rtpcMax));
                b.AddRange(GetBytes(rtpcHold));
                b.Add(nonRTPCMode);
                b.Add(nonRTPCScope);
                b.Add(nonRTPCApplyDownstreamVolume);
                b.AddRange(GetBytes(nonRTPCGameParamID));
                return b;
            }
        }

        public class PluginPropertyValue : ISerializable
        {
            public ulong propertyID;
            public byte rtpcAccum;
            public float value;

            public void Deserialize(WwiseData wd)
            {
                propertyID = ReadVariableInt(wd);
                rtpcAccum = ReadUInt8(wd);
                value = ReadSingle(wd);
            }

            public IEnumerable<byte> Serialize()
            {
                List<byte> b = new();
                b.AddRange(GetVariableIntBytes(propertyID));
                b.Add(rtpcAccum);
                b.AddRange(GetBytes(value));
                return b;
            }
        }

        public class Bus : HircItem
        {
            public uint overrideBusId;
            public uint idDeviceShareset;
            public BusInitialParams busInitialParams;
            public int recoveryTime;
            public float maxDuckVolume;
            public uint ducksCount;
            public List<DuckInfo> toDuckList;
            public BusInitialFxParams busInitialFxParams;
            public byte overrideAttachmentParams;
            public InitialRTPC initialRTPC;
            public StateChunk stateChunk;

            public override void Deserialize(WwiseData wd)
            {
                base.Deserialize(wd);

                busInitialParams = new();
                toDuckList = new();
                busInitialFxParams = new();
                initialRTPC = new();
                stateChunk = new();
                overrideBusId = ReadUInt32(wd);
                if (overrideBusId == 0)
                    idDeviceShareset = ReadUInt32(wd);
                busInitialParams.Deserialize(wd);
                recoveryTime = ReadInt32(wd);
                maxDuckVolume = ReadSingle(wd);
                ducksCount = ReadUInt32(wd);
                for (int i = 0; i < ducksCount; i++)
                {
                    DuckInfo di = new();
                    di.Deserialize(wd);
                    toDuckList.Add(di);
                }
                busInitialFxParams.Deserialize(wd);
                overrideAttachmentParams = ReadUInt8(wd);
                initialRTPC.Deserialize(wd);
                stateChunk.Deserialize(wd);
            }

            public override IEnumerable<byte> Serialize()
            {
                List<byte> b0 = new();
                b0.AddRange(GetBytes(overrideBusId));
                if (overrideBusId == 0)
                    b0.AddRange(GetBytes(idDeviceShareset));
                b0.AddRange(busInitialParams.Serialize());
                b0.AddRange(GetBytes(recoveryTime));
                b0.AddRange(GetBytes(maxDuckVolume));
                ducksCount = (uint)toDuckList.Count;
                b0.AddRange(GetBytes(ducksCount));
                foreach (DuckInfo di in toDuckList)
                    b0.AddRange(di.Serialize());
                b0.AddRange(busInitialFxParams.Serialize());
                b0.Add(overrideAttachmentParams);
                b0.AddRange(initialRTPC.Serialize());
                b0.AddRange(stateChunk.Serialize());
                sectionSize = (uint)(b0.Count + 4);

                List<byte> b = new();
                b.AddRange(base.Serialize());
                b.AddRange(b0);
                return b;
            }
        }

        public class BusInitialParams : ISerializable
        {
            public PropBundle0 propBundle0;
            public PositioningParams positioningParams;
            public AuxParams auxParams;
            public bool killNewest;
            public bool useVirtualBehavior;
            public bool isMaxNumInstIgnoreParent;
            public bool isBackgroundMusic;
            public ushort maxNumInstance;
            public uint channelConfig;
            public bool isHdrBus;
            public bool hdrReleaseModeExponential;

            public void Deserialize(WwiseData wd)
            {
                propBundle0 = new();
                positioningParams = new();
                auxParams = new();
                propBundle0.Deserialize(wd);
                positioningParams.Deserialize(wd);
                auxParams.Deserialize(wd);
                bool[] flags0 = ReadFlags(wd);
                killNewest = flags0[0];
                useVirtualBehavior = flags0[1];
                isMaxNumInstIgnoreParent = flags0[2];
                isBackgroundMusic = flags0[3];
                maxNumInstance = ReadUInt16(wd);
                channelConfig = ReadUInt32(wd);
                bool[] flags1 = ReadFlags(wd);
                isHdrBus = flags1[0];
                hdrReleaseModeExponential = flags1[1];
            }

            public IEnumerable<byte> Serialize()
            {
                List<byte> b = new();
                b.AddRange(propBundle0.Serialize());
                b.AddRange(positioningParams.Serialize());
                b.AddRange(auxParams.Serialize());
                bool[] flags0 = {
                    killNewest,
                    useVirtualBehavior,
                    isMaxNumInstIgnoreParent,
                    isBackgroundMusic
                };
                b.Add(GetByte(flags0));
                b.AddRange(GetBytes(maxNumInstance));
                b.AddRange(GetBytes(channelConfig));
                bool[] flags1 = {
                    isHdrBus,
                    hdrReleaseModeExponential
                };
                b.Add(GetByte(flags1));
                return b;
            }
        }

        public class DuckInfo : ISerializable
        {
            public uint busID;
            public float duckVolume;
            public int fadeOutTime;
            public int fadeInTime;
            public byte fadeCurve;
            public byte targetProp;

            public void Deserialize(WwiseData wd)
            {
                busID = ReadUInt32(wd);
                duckVolume = ReadSingle(wd);
                fadeOutTime = ReadInt32(wd);
                fadeInTime = ReadInt32(wd);
                fadeCurve = ReadUInt8(wd);
                targetProp = ReadUInt8(wd);
            }

            public IEnumerable<byte> Serialize()
            {
                List<byte> b = new();
                b.AddRange(GetBytes(busID));
                b.AddRange(GetBytes(duckVolume));
                b.AddRange(GetBytes(fadeOutTime));
                b.AddRange(GetBytes(fadeInTime));
                b.Add(fadeCurve);
                b.Add(targetProp);
                return b;
            }
        }

        public class BusInitialFxParams : ISerializable
        {
            public byte fxCount;
            public byte bitsFXBypass;
            public List<FXChunk> fxChunk;
            public uint fxID0;
            public byte IsShareSet0;

            public void Deserialize(WwiseData wd)
            {
                fxCount = ReadUInt8(wd);
                if (fxCount > 0)
                {
                    fxChunk = new();
                    bitsFXBypass = ReadUInt8(wd);
                    for (int i = 0; i < fxCount; i++)
                    {
                        FXChunk fxc = new();
                        fxc.Deserialize(wd);
                        fxChunk.Add(fxc);
                    }
                }
                fxID0 = ReadUInt32(wd);
                IsShareSet0 = ReadUInt8(wd);
            }

            public IEnumerable<byte> Serialize()
            {
                List<byte> b = new();
                if (fxChunk != null)
                    fxCount = (byte)fxChunk.Count;
                b.Add(fxCount);
                if (fxCount > 0)
                {
                    b.Add(bitsFXBypass);
                    foreach (FXChunk fxc in fxChunk)
                        b.AddRange(fxc.Serialize());
                }
                b.AddRange(GetBytes(fxID0));
                b.Add(IsShareSet0);
                return b;
            }
        }

        public class FXChunk : ISerializable
        {
            public byte fxIndex;
            public uint fxID;
            public byte isShareSet;
            public byte isRendered;

            public void Deserialize(WwiseData wd)
            {
                fxIndex = ReadUInt8(wd);
                fxID = ReadUInt32(wd);
                isShareSet = ReadUInt8(wd);
                isRendered = ReadUInt8(wd);
            }

            public IEnumerable<byte> Serialize()
            {
                List<byte> b = new();
                b.Add(fxIndex);
                b.AddRange(GetBytes(fxID));
                b.Add(isShareSet);
                b.Add(isRendered);
                return b;
            }
        }

        public class LayerCntr : HircItem
        {
            public NodeBaseParams nodeBaseParams;
            public Children children;
            public uint layersCount;
            public List<Layer> layers;
            public byte isContinuousValidation;

            public override void Deserialize(WwiseData wd)
            {
                base.Deserialize(wd);

                nodeBaseParams = new();
                children = new();
                layers = new();
                nodeBaseParams.Deserialize(wd);
                children.Deserialize(wd);
                layersCount = ReadUInt32(wd);
                for (int i = 0; i < layersCount; i++)
                {
                    Layer l = new();
                    l.Deserialize(wd);
                    layers.Add(l);
                }
                isContinuousValidation = ReadUInt8(wd);
            }

            public override IEnumerable<byte> Serialize()
            {
                List<byte> b0 = new();
                b0.AddRange(nodeBaseParams.Serialize());
                b0.AddRange(children.Serialize());
                layersCount = (uint)layers.Count;
                b0.AddRange(GetBytes(layersCount));
                foreach (Layer l in layers)
                    b0.AddRange(l.Serialize());
                b0.Add(isContinuousValidation);
                sectionSize = (uint)(b0.Count + 4);

                List<byte> b = new();
                b.AddRange(base.Serialize());
                b.AddRange(b0);
                return b;
            }
        }

        public class Layer : ISerializable
        {
            public uint layerID;
            public LayerInitialValues layerInitialValues;

            public void Deserialize(WwiseData wd)
            {
                layerInitialValues = new();
                layerID = ReadUInt32(wd);
                layerInitialValues.Deserialize(wd);
            }

            public IEnumerable<byte> Serialize()
            {
                List<byte> b = new();
                b.AddRange(GetBytes(layerID));
                b.AddRange(layerInitialValues.Serialize());
                return b;
            }
        }

        public class LayerInitialValues : ISerializable
        {
            public InitialRTPC initialRTPC;
            public uint rtpcID;
            public byte rtpcType;
            public uint assocCount;
            public List<AssociatedChildData> assocs;

            public void Deserialize(WwiseData wd)
            {
                initialRTPC = new();
                assocs = new();
                initialRTPC.Deserialize(wd);
                rtpcID = ReadUInt32(wd);
                rtpcType = ReadUInt8(wd);
                assocCount = ReadUInt32(wd);
                for (int i = 0; i < assocCount; i++)
                {
                    AssociatedChildData acd = new();
                    acd.Deserialize(wd);
                    assocs.Add(acd);
                }
            }

            public IEnumerable<byte> Serialize()
            {
                List<byte> b = new();
                b.AddRange(initialRTPC.Serialize());
                b.AddRange(GetBytes(rtpcID));
                b.Add(rtpcType);
                assocCount = (uint)assocs.Count;
                b.AddRange(GetBytes(assocCount));
                foreach (AssociatedChildData acd in assocs)
                    b.AddRange(acd.Serialize());
                return b;
            }
        }

        public class AssociatedChildData : ISerializable
        {
            public uint associatedChildID;
            public uint curveSize;
            public List<RTPCGraphPoint> pRTPCMgr;

            public void Deserialize(WwiseData wd)
            {
                pRTPCMgr = new();
                associatedChildID = ReadUInt32(wd);
                curveSize = ReadUInt32(wd);
                for (int i = 0; i < curveSize; i++)
                {
                    RTPCGraphPoint rtpcgp = new();
                    rtpcgp.Deserialize(wd);
                    pRTPCMgr.Add(rtpcgp);
                }
            }

            public IEnumerable<byte> Serialize()
            {
                List<byte> b = new();
                b.AddRange(GetBytes(associatedChildID));
                curveSize = (uint)pRTPCMgr.Count;
                b.AddRange(GetBytes(curveSize));
                foreach (RTPCGraphPoint rtpcgp in pRTPCMgr)
                    b.AddRange(rtpcgp.Serialize());
                return b;
            }
        }

        public class MusicSegment : HircItem
        {
            public MusicNodeParams musicNodeParams;
            public double duration;
            public uint markersCount;
            public List<MusicMarkerWwise> arrayMarkers;

            public override void Deserialize(WwiseData wd)
            {
                base.Deserialize(wd);

                musicNodeParams = new();
                arrayMarkers = new();
                musicNodeParams.Deserialize(wd);
                duration = ReadDouble(wd);
                markersCount = ReadUInt32(wd);
                for (int i = 0; i < markersCount; i++)
                {
                    MusicMarkerWwise mmw = new();
                    mmw.Deserialize(wd);
                    arrayMarkers.Add(mmw);
                }
            }

            public override IEnumerable<byte> Serialize()
            {
                List<byte> b0 = new();
                b0.AddRange(musicNodeParams.Serialize());
                b0.AddRange(GetBytes(duration));
                markersCount = (uint)arrayMarkers.Count;
                b0.AddRange(GetBytes(markersCount));
                foreach (MusicMarkerWwise mmw in arrayMarkers)
                    b0.AddRange(mmw.Serialize());
                sectionSize = (uint)(b0.Count + 4);

                List<byte> b = new();
                b.AddRange(base.Serialize());
                b.AddRange(b0);
                return b;
            }
        }

        public class MusicNodeParams : ISerializable
        {
            public bool overrideParentMidiTempo;
            public bool overrideParentMidiTarget;
            public bool midiTargetTypeBus;
            public NodeBaseParams nodeBaseParams;
            public Children children;
            public MeterInfo meterInfo;
            public byte meterInfoFlag;
            public uint stingersCount;
            public List<Unk> stingers;

            public void Deserialize(WwiseData wd)
            {
                nodeBaseParams = new();
                children = new();
                meterInfo = new();
                stingers = new();
                bool[] flags = ReadFlags(wd);
                overrideParentMidiTempo = flags[1];
                overrideParentMidiTarget = flags[2];
                midiTargetTypeBus = flags[3];
                nodeBaseParams.Deserialize(wd);
                children.Deserialize(wd);
                meterInfo.Deserialize(wd);
                meterInfoFlag = ReadUInt8(wd);
                stingersCount = ReadUInt32(wd);
                for (int i = 0; i < stingersCount; i++)
                {
                    Unk u = new();
                    u.Deserialize(wd);
                    stingers.Add(u);
                }
            }

            public IEnumerable<byte> Serialize()
            {
                List<byte> b = new();
                bool[] flags = { false,
                    overrideParentMidiTempo,
                    overrideParentMidiTarget,
                    midiTargetTypeBus
                };
                b.Add(GetByte(flags));
                b.AddRange(nodeBaseParams.Serialize());
                b.AddRange(children.Serialize());
                b.AddRange(meterInfo.Serialize());
                b.Add(meterInfoFlag);
                stingersCount = (uint)stingers.Count;
                b.AddRange(GetBytes(stingersCount));
                foreach (Unk u in stingers)
                    b.AddRange(u.Serialize());
                return b;
            }
        }

        public class MeterInfo : ISerializable
        {
            public double gridPeriod;
            public double gridOffset;
            public float tempo;
            public byte timeSigNumBeatsBar;
            public byte timeSigBeatValue;

            public void Deserialize(WwiseData wd)
            {
                gridPeriod = ReadDouble(wd);
                gridOffset = ReadDouble(wd);
                tempo = ReadSingle(wd);
                timeSigNumBeatsBar = ReadUInt8(wd);
                timeSigBeatValue = ReadUInt8(wd);
            }

            public IEnumerable<byte> Serialize()
            {
                List<byte> b = new();
                b.AddRange(GetBytes(gridPeriod));
                b.AddRange(GetBytes(gridOffset));
                b.AddRange(GetBytes(tempo));
                b.Add(timeSigNumBeatsBar);
                b.Add(timeSigBeatValue);
                return b;
            }
        }

        public class MusicMarkerWwise : ISerializable
        {
            public uint id;
            public double position;
            public uint stringSize;

            public void Deserialize(WwiseData wd)
            {
                id = ReadUInt32(wd);
                position = ReadDouble(wd);
                stringSize = ReadUInt32(wd);
            }

            public IEnumerable<byte> Serialize()
            {
                List<byte> b = new();
                b.AddRange(GetBytes(id));
                b.AddRange(GetBytes(position));
                b.AddRange(GetBytes(stringSize));
                return b;
            }
        }

        public class MusicTrack : HircItem
        {
            public bool overrideParentMidiTempo;
            public bool overrideParentMidiTarget;
            public bool midiTargetTypeBus;
            public uint sourceCount;
            public List<BankSourceData> source;
            public uint playlistItemCount;
            public List<TrackSrcInfo> playlist;
            public uint subTrackCount;
            public uint clipAutomationItemCount;
            public List<ClipAutomation> items;
            public NodeBaseParams nodeBaseParams;
            public byte trackType;
            public SwitchParams switchParams;
            public TransParams transParams;
            public int lookAheadTime;

            public override void Deserialize(WwiseData wd)
            {
                base.Deserialize(wd);

                source = new();
                playlist = new();
                items = new();
                nodeBaseParams = new();
                bool[] flags = ReadFlags(wd);
                overrideParentMidiTempo = flags[1];
                overrideParentMidiTarget = flags[2];
                midiTargetTypeBus = flags[3];
                sourceCount = ReadUInt32(wd);
                for (int i = 0; i < sourceCount; i++)
                {
                    BankSourceData bsd = new();
                    bsd.Deserialize(wd);
                    source.Add(bsd);
                }
                playlistItemCount = ReadUInt32(wd);
                for (int i = 0; i < playlistItemCount; i++)
                {
                    TrackSrcInfo tsi = new();
                    tsi.Deserialize(wd);
                    playlist.Add(tsi);
                }
                subTrackCount = ReadUInt32(wd);
                clipAutomationItemCount = ReadUInt32(wd);
                for (int i = 0; i < clipAutomationItemCount; i++)
                {
                    ClipAutomation ca = new();
                    ca.Deserialize(wd);
                    items.Add(ca);
                }
                nodeBaseParams.Deserialize(wd);
                trackType = ReadUInt8(wd);
                switch (trackType)
                {
                    case 3:
                        switchParams = new();
                        transParams = new();
                        switchParams.Deserialize(wd);
                        transParams.Deserialize(wd);
                        break;
                }
                lookAheadTime = ReadInt32(wd);
            }

            public override IEnumerable<byte> Serialize()
            {
                List<byte> b0 = new();
                bool[] flags = { false,
                    overrideParentMidiTempo,
                    overrideParentMidiTarget,
                    midiTargetTypeBus
                };
                b0.Add(GetByte(flags));
                sourceCount = (uint)source.Count;
                b0.AddRange(GetBytes(sourceCount));
                foreach (BankSourceData bsd in source)
                    b0.AddRange(bsd.Serialize());
                playlistItemCount = (uint)playlist.Count;
                b0.AddRange(GetBytes(playlistItemCount));
                foreach (TrackSrcInfo tsi in playlist)
                    b0.AddRange(tsi.Serialize());
                b0.AddRange(GetBytes(subTrackCount));
                clipAutomationItemCount = (uint)items.Count;
                b0.AddRange(GetBytes(clipAutomationItemCount));
                foreach (ClipAutomation ca in items)
                    b0.AddRange(ca.Serialize());
                b0.AddRange(nodeBaseParams.Serialize());
                b0.Add(trackType);
                if (switchParams != null)
                    b0.AddRange(switchParams.Serialize());
                if (transParams != null)
                    b0.AddRange(transParams.Serialize());
                b0.AddRange(GetBytes(lookAheadTime));
                sectionSize = (uint)(b0.Count + 4);

                List<byte> b = new();
                b.AddRange(base.Serialize());
                b.AddRange(b0);
                return b;
            }
        }

        public class TrackSrcInfo : ISerializable
        {
            public uint trackID;
            public uint sourceID;
            public uint eventID;
            public double playAt;
            public double beginTrimOffset;
            public double endTrimOffset;
            public double srcDuration;

            public void Deserialize(WwiseData wd)
            {
                trackID = ReadUInt32(wd);
                sourceID = ReadUInt32(wd);
                eventID = ReadUInt32(wd);
                playAt = ReadDouble(wd);
                beginTrimOffset = ReadDouble(wd);
                endTrimOffset = ReadDouble(wd);
                srcDuration = ReadDouble(wd);
            }

            public IEnumerable<byte> Serialize()
            {
                List<byte> b = new();
                b.AddRange(GetBytes(trackID));
                b.AddRange(GetBytes(sourceID));
                b.AddRange(GetBytes(eventID));
                b.AddRange(GetBytes(playAt));
                b.AddRange(GetBytes(beginTrimOffset));
                b.AddRange(GetBytes(endTrimOffset));
                b.AddRange(GetBytes(srcDuration));
                return b;
            }
        }

        public class ClipAutomation : ISerializable
        {
            public uint clipIndex;
            public uint autoType;
            public uint pointsCount;
            public List<RTPCGraphPoint> graphPoints;

            public void Deserialize(WwiseData wd)
            {
                graphPoints = new();
                clipIndex = ReadUInt32(wd);
                autoType = ReadUInt32(wd);
                pointsCount = ReadUInt32(wd);
                for (int i = 0; i < pointsCount; i++)
                {
                    RTPCGraphPoint rtpcgp = new();
                    rtpcgp.Deserialize(wd);
                    graphPoints.Add(rtpcgp);
                }
            }

            public IEnumerable<byte> Serialize()
            {
                List<byte> b = new();
                b.AddRange(GetBytes(clipIndex));
                b.AddRange(GetBytes(autoType));
                pointsCount = (uint)graphPoints.Count;
                b.AddRange(GetBytes(pointsCount));
                foreach (RTPCGraphPoint rtpcgp in graphPoints)
                    b.AddRange(rtpcgp.Serialize());
                return b;
            }
        }

        public class SwitchParams : ISerializable
        {
            public byte groupType;
            public uint groupID;
            public uint defaultSwitch;
            public uint switchAssocCount;
            public List<TrackSwitchAssoc> switchAssoc;

            public void Deserialize(WwiseData wd)
            {
                switchAssoc = new();
                groupType = ReadUInt8(wd);
                groupID = ReadUInt32(wd);
                defaultSwitch = ReadUInt32(wd);
                switchAssocCount = ReadUInt32(wd);
                for (int i = 0; i < switchAssocCount; i++)
                {
                    TrackSwitchAssoc tsa = new();
                    tsa.Deserialize(wd);
                    switchAssoc.Add(tsa);
                }
            }

            public IEnumerable<byte> Serialize()
            {
                List<byte> b = new();
                b.Add(groupType);
                b.AddRange(GetBytes(groupID));
                b.AddRange(GetBytes(defaultSwitch));
                switchAssocCount = (uint)switchAssoc.Count;
                b.AddRange(GetBytes(switchAssocCount));
                foreach (TrackSwitchAssoc tsa in switchAssoc)
                    b.AddRange(tsa.Serialize());
                return b;
            }
        }

        public class TrackSwitchAssoc : ISerializable
        {
            public uint switchAssoc;

            public void Deserialize(WwiseData wd)
            {
                switchAssoc = ReadUInt32(wd);
            }

            public IEnumerable<byte> Serialize()
            {
                return GetBytes(switchAssoc);
            }
        }

        public class TransParams : ISerializable
        {
            public FadeParams srcFadeParams;
            public uint syncType;
            public uint cueFilterHash;
            public FadeParams destFadeParams;

            public void Deserialize(WwiseData wd)
            {
                srcFadeParams = new();
                destFadeParams = new();
                srcFadeParams.Deserialize(wd);
                syncType = ReadUInt32(wd);
                cueFilterHash = ReadUInt32(wd);
                destFadeParams.Deserialize(wd);
            }

            public IEnumerable<byte> Serialize()
            {
                List<byte> b = new();
                b.AddRange(srcFadeParams.Serialize());
                b.AddRange(GetBytes(syncType));
                b.AddRange(GetBytes(cueFilterHash));
                b.AddRange(destFadeParams.Serialize());
                return b;
            }
        }

        public class FadeParams : ISerializable
        {
            public int transitionTime;
            public uint fadeCurve;
            public int fadeOffset;

            public void Deserialize(WwiseData wd)
            {
                transitionTime = ReadInt32(wd);
                fadeCurve = ReadUInt32(wd);
                fadeOffset = ReadInt32(wd);
            }

            public IEnumerable<byte> Serialize()
            {
                List<byte> b = new();
                b.AddRange(GetBytes(transitionTime));
                b.AddRange(GetBytes(fadeCurve));
                b.AddRange(GetBytes(fadeOffset));
                return b;
            }
        }

        public class MusicSwitchCntr : HircItem
        {
            public MusicTransNodeParams musicTransNodeParams;
            public byte isContinuePlayback;
            public uint treeDepth;
            public List<GameSync> arguments;
            public uint treeDataSize;
            public byte mode;
            public Node decisionTree;

            public override void Deserialize(WwiseData wd)
            {
                base.Deserialize(wd);

                musicTransNodeParams = new();
                arguments = new();
                musicTransNodeParams.Deserialize(wd);
                isContinuePlayback = ReadUInt8(wd);
                treeDepth = ReadUInt32(wd);
                for (int i = 0; i < treeDepth; i++)
                {
                    GameSync gs = new();
                    gs.Deserialize(wd);
                    arguments.Add(gs);
                }
                foreach (GameSync gs in arguments)
                    gs.DeserializeGroupType(wd);
                treeDataSize = ReadUInt32(wd);
                mode = ReadUInt8(wd);
                decisionTree = new((int)treeDepth);
                decisionTree.Deserialize(wd);
                decisionTree.DeserializeChildren(wd);
            }

            public override IEnumerable<byte> Serialize()
            {
                List<byte> b0 = new();
                b0.AddRange(musicTransNodeParams.Serialize());
                b0.Add(isContinuePlayback);
                b0.AddRange(GetBytes(treeDepth));
                foreach (GameSync gs in arguments)
                    b0.AddRange(gs.Serialize());
                foreach (GameSync gs in arguments)
                    b0.AddRange(gs.SerializeGroupType());

                List<byte> b1 = new();
                decisionTree.childrenIdx = 1;
                b1.AddRange(decisionTree.Serialize());
                b1.AddRange(decisionTree.SerializeChildren());
                treeDataSize = (uint)b1.Count;
                b0.AddRange(GetBytes(treeDataSize));

                b0.Add(mode);
                b0.AddRange(b1);
                sectionSize = (uint)(b0.Count + 4);

                List<byte> b = new();
                b.AddRange(base.Serialize());
                b.AddRange(b0);
                return b;
            }
        }

        public class GameSync : ISerializable
        {
            public uint group;
            public byte groupType;

            public void Deserialize(WwiseData wd)
            {
                group = ReadUInt32(wd);
            }

            public void DeserializeGroupType(WwiseData wd)
            {
                groupType = ReadUInt8(wd);
            }

            public IEnumerable<byte> Serialize()
            {
                return GetBytes(group);
            }

            public IEnumerable<byte> SerializeGroupType()
            {
                return new byte[] { groupType };
            }
        }

        public class Node : ISerializable
        {
            public uint key;
            public ushort childrenIdx;
            public ushort childrenCount;
            public uint audioNodeId;
            public ushort weight;
            public ushort probability;
            public List<Node> nodes;

            public int level;

            public Node(int level)
            {
                this.level = level;
            }

            public void Deserialize(WwiseData wd)
            {
                nodes = new();
                key = ReadUInt32(wd);
                if (level > 0)
                {
                    childrenIdx = ReadUInt16(wd);
                    childrenCount = ReadUInt16(wd);
                }
                else
                    audioNodeId = ReadUInt32(wd);
                weight = ReadUInt16(wd);
                probability = ReadUInt16(wd);
                for (int i = 0; level > 0 && i < childrenCount; i++)
                    nodes.Add(new(level - 1));
            }

            public void DeserializeChildren(WwiseData wd)
            {
                if (level == 0)
                    return;
                foreach (Node n in nodes)
                    n.Deserialize(wd);
                foreach (Node n in nodes)
                    n.DeserializeChildren(wd);
            }

            public IEnumerable<byte> Serialize()
            {
                nodes.Sort((o0, o1) => o0.key.CompareTo(o1.key));

                List<byte> b = new();
                b.AddRange(GetBytes(key));
                if (level > 0)
                {
                    b.AddRange(GetBytes(childrenIdx));
                    childrenCount = (ushort)nodes.Count;
                    b.AddRange(GetBytes(childrenCount));
                }
                else
                    b.AddRange(GetBytes(audioNodeId));
                b.AddRange(GetBytes(weight));
                b.AddRange(GetBytes(probability));
                return b;
            }

            public IEnumerable<byte> SerializeChildren()
            {
                List<byte> b = new();
                if (level == 0)
                    return b;
                int nodeIdx = childrenIdx + childrenCount;
                foreach (Node n in nodes)
                {
                    if (n.level > 0)
                        n.childrenIdx = (ushort)nodeIdx;
                    b.AddRange(n.Serialize());
                    nodeIdx += n.GetIdxIncrement();
                }
                foreach (Node n in nodes)
                    b.AddRange(n.SerializeChildren());
                return b;
            }

            public int GetIdxIncrement()
            {
                if (level == 0)
                    return 0;
                int inc = nodes.Count;
                foreach (Node n in nodes)
                    inc += n.GetIdxIncrement();
                return inc;
            }
        }

        public class MusicRanSeqCntr : HircItem
        {
            public MusicTransNodeParams musicTransNodeParams;
            public uint playlistItemsCount;
            public List<MusicRanSeqPlaylistItem> playList;

            public override void Deserialize(WwiseData wd)
            {
                base.Deserialize(wd);

                musicTransNodeParams = new();
                playList = new();
                musicTransNodeParams.Deserialize(wd);
                playlistItemsCount = ReadUInt32(wd);
                for (int i = 0; i < playlistItemsCount; i++)
                {
                    MusicRanSeqPlaylistItem mrspi = new();
                    mrspi.Deserialize(wd);
                    i += mrspi.GetChildrenCount();
                    playList.Add(mrspi);
                }
            }

            public override IEnumerable<byte> Serialize()
            {
                List<byte> b0 = new();
                b0.AddRange(musicTransNodeParams.Serialize());
                playlistItemsCount = 0;
                foreach (MusicRanSeqPlaylistItem mrspi in playList)
                    playlistItemsCount += 1 + (uint)mrspi.GetChildrenCount();
                b0.AddRange(GetBytes(playlistItemsCount));
                foreach (MusicRanSeqPlaylistItem mrspi in playList)
                    b0.AddRange(mrspi.Serialize());
                sectionSize = (uint)(b0.Count + 4);

                List<byte> b = new();
                b.AddRange(base.Serialize());
                b.AddRange(b0);
                return b;
            }
        }

        public class MusicTransNodeParams : ISerializable
        {
            public MusicNodeParams musicNodeParams;
            public uint rulesCount;
            public List<MusicTransitionRule> rules;

            public void Deserialize(WwiseData wd)
            {
                musicNodeParams = new();
                rules = new();
                musicNodeParams.Deserialize(wd);
                rulesCount = ReadUInt32(wd);
                for (int i = 0; i < rulesCount; i++)
                {
                    MusicTransitionRule mtr = new();
                    mtr.Deserialize(wd);
                    rules.Add(mtr);
                }
            }

            public IEnumerable<byte> Serialize()
            {
                List<byte> b = new();
                b.AddRange(musicNodeParams.Serialize());
                rulesCount = (uint)rules.Count;
                b.AddRange(GetBytes(rulesCount));
                foreach (MusicTransitionRule mtr in rules)
                    b.AddRange(mtr.Serialize());
                return b;
            }
        }

        public class MusicTransitionRule : ISerializable
        {
            public uint srcCount;
            public List<uint> srcIDs;
            public uint dstCount;
            public List<uint> dstIDs;
            public MusicTransSrcRule musicTransSrcRule;
            public MusicTransDstRule musicTransDstRule;
            public byte allocTransObjectFlag;
            public MusicTransitionObject musicTransitionObject;

            public void Deserialize(WwiseData wd)
            {
                srcIDs = new();
                dstIDs = new();
                musicTransSrcRule = new();
                musicTransDstRule = new();
                srcCount = ReadUInt32(wd);
                for (int i = 0; i < srcCount; i++)
                    srcIDs.Add(ReadUInt32(wd));
                dstCount = ReadUInt32(wd);
                for (int i = 0; i < dstCount; i++)
                    dstIDs.Add(ReadUInt32(wd));
                musicTransSrcRule.Deserialize(wd);
                musicTransDstRule.Deserialize(wd);
                allocTransObjectFlag = ReadUInt8(wd);
                if (allocTransObjectFlag > 0)
                {
                    musicTransitionObject = new();
                    musicTransitionObject.Deserialize(wd);
                }
            }

            public IEnumerable<byte> Serialize()
            {
                srcIDs.Sort();
                dstIDs.Sort();

                List<byte> b = new();
                srcCount = (uint)srcIDs.Count;
                b.AddRange(GetBytes(srcCount));
                foreach (uint i in srcIDs)
                    b.AddRange(GetBytes(i));
                dstCount = (uint)dstIDs.Count;
                b.AddRange(GetBytes(dstCount));
                foreach (uint i in dstIDs)
                    b.AddRange(GetBytes(i));
                b.AddRange(musicTransSrcRule.Serialize());
                b.AddRange(musicTransDstRule.Serialize());
                b.Add(allocTransObjectFlag);
                if (musicTransitionObject != null)
                    b.AddRange(musicTransitionObject.Serialize());
                return b;
            }
        }

        public class MusicTransSrcRule : ISerializable
        {
            public int transitionTime;
            public uint fadeCurve;
            public int fadeOffset;
            public uint syncType;
            public uint cueFilterHash;
            public byte playPostExit;

            public void Deserialize(WwiseData wd)
            {
                transitionTime = ReadInt32(wd);
                fadeCurve = ReadUInt32(wd);
                fadeOffset = ReadInt32(wd);
                syncType = ReadUInt32(wd);
                cueFilterHash = ReadUInt32(wd);
                playPostExit = ReadUInt8(wd);
            }

            public IEnumerable<byte> Serialize()
            {
                List<byte> b = new();
                b.AddRange(GetBytes(transitionTime));
                b.AddRange(GetBytes(fadeCurve));
                b.AddRange(GetBytes(fadeOffset));
                b.AddRange(GetBytes(syncType));
                b.AddRange(GetBytes(cueFilterHash));
                b.Add(playPostExit);
                return b;
            }
        }

        public class MusicTransDstRule : ISerializable
        {
            public int transitionTime;
            public uint fadeCurve;
            public int fadeOffset;
            public uint cueFilterHash;
            public uint jumpToID;
            public ushort jumpToType;
            public ushort entryType;
            public byte playPreEntry;
            public byte destMatchSourceCueName;

            public void Deserialize(WwiseData wd)
            {
                transitionTime = ReadInt32(wd);
                fadeCurve = ReadUInt32(wd);
                fadeOffset = ReadInt32(wd);
                cueFilterHash = ReadUInt32(wd);
                jumpToID = ReadUInt32(wd);
                jumpToType = ReadUInt16(wd);
                entryType = ReadUInt16(wd);
                playPreEntry = ReadUInt8(wd);
                destMatchSourceCueName = ReadUInt8(wd);
            }

            public IEnumerable<byte> Serialize()
            {
                List<byte> b = new();
                b.AddRange(GetBytes(transitionTime));
                b.AddRange(GetBytes(fadeCurve));
                b.AddRange(GetBytes(fadeOffset));
                b.AddRange(GetBytes(cueFilterHash));
                b.AddRange(GetBytes(jumpToID));
                b.AddRange(GetBytes(jumpToType));
                b.AddRange(GetBytes(entryType));
                b.Add(playPreEntry);
                b.Add(destMatchSourceCueName);
                return b;
            }
        }

        public class MusicTransitionObject : ISerializable
        {
            public uint segmentID;
            public FadeParams fadeInParams;
            public FadeParams fadeOutParams;
            public byte playPreEntry;
            public byte playPostExit;

            public void Deserialize(WwiseData wd)
            {
                fadeInParams = new();
                fadeOutParams = new();
                segmentID = ReadUInt32(wd);
                fadeInParams.Deserialize(wd);
                fadeOutParams.Deserialize(wd);
                playPreEntry = ReadUInt8(wd);
                playPostExit = ReadUInt8(wd);
            }

            public IEnumerable<byte> Serialize()
            {
                List<byte> b = new();
                b.AddRange(GetBytes(segmentID));
                b.AddRange(fadeInParams.Serialize());
                b.AddRange(fadeOutParams.Serialize());
                b.Add(playPreEntry);
                b.Add(playPostExit);
                return b;
            }
        }

        public class MusicRanSeqPlaylistItem : WwiseObject
        {
            public uint segmentID;
            public uint playlistItemID;
            public uint childrenCount;
            public uint rsType;
            public short loop;
            public short loopMin;
            public short loopMax;
            public uint weight;
            public ushort avoidRepeatCount;
            public byte isUsingWeight;
            public byte isShuffle;
            public List<MusicRanSeqPlaylistItem> playList;

            public override void Deserialize(WwiseData wd)
            {
                playList = new();
                segmentID = ReadUInt32(wd);
                playlistItemID = ReadUInt32(wd);
                wd.objectsByID[playlistItemID] = this;
                childrenCount = ReadUInt32(wd);
                rsType = ReadUInt32(wd);
                loop = ReadInt16(wd);
                loopMin = ReadInt16(wd);
                loopMax = ReadInt16(wd);
                weight = ReadUInt32(wd);
                avoidRepeatCount = ReadUInt16(wd);
                isUsingWeight = ReadUInt8(wd);
                isShuffle = ReadUInt8(wd);
                for (int i = 0; i < childrenCount; i++)
                {
                    MusicRanSeqPlaylistItem mrspi = new();
                    mrspi.Deserialize(wd);
                    playList.Add(mrspi);
                }
            }

            public override IEnumerable<byte> Serialize()
            {
                List<byte> b = new();
                b.AddRange(GetBytes(segmentID));
                b.AddRange(GetBytes(playlistItemID));
                childrenCount = (uint)playList.Count;
                b.AddRange(GetBytes(childrenCount));
                b.AddRange(GetBytes(rsType));
                b.AddRange(GetBytes(loop));
                b.AddRange(GetBytes(loopMin));
                b.AddRange(GetBytes(loopMax));
                b.AddRange(GetBytes(weight));
                b.AddRange(GetBytes(avoidRepeatCount));
                b.Add(isUsingWeight);
                b.Add(isShuffle);
                foreach (MusicRanSeqPlaylistItem mrspi in playList)
                    b.AddRange(mrspi.Serialize());
                return b;
            }

            public int GetChildrenCount()
            {
                int n = 0;
                foreach (MusicRanSeqPlaylistItem mrspi in playList)
                    n += 1 + mrspi.GetChildrenCount();
                return n;
            }
        }

        public class Attenuation : HircItem
        {
            public byte isConeEnabled;
            public ConeParams coneParams;
            public sbyte[] curveToUse;
            public byte curvesCount;
            public List<ConversionTable> curves;
            public InitialRTPC initialRTPC;

            public override void Deserialize(WwiseData wd)
            {
                base.Deserialize(wd);

                curves = new();
                initialRTPC = new();
                isConeEnabled = ReadUInt8(wd);
                if (isConeEnabled > 0)
                {
                    coneParams = new();
                    coneParams.Deserialize(wd);
                }
                curveToUse = new sbyte[7];
                curveToUse = ReadInt8Array(wd, 7);
                curvesCount = ReadUInt8(wd);
                for (int i = 0; i < curvesCount; i++)
                {
                    ConversionTable ct = new();
                    ct.Deserialize(wd);
                    curves.Add(ct);
                }
                initialRTPC.Deserialize(wd);
            }

            public override IEnumerable<byte> Serialize()
            {
                List<byte> b0 = new();
                isConeEnabled = (byte)(coneParams == null ? 0 : 1);
                b0.Add(isConeEnabled);
                if (coneParams != null)
                    b0.AddRange(coneParams.Serialize());
                b0.AddRange(GetBytes(curveToUse));
                curvesCount = (byte)curves.Count;
                b0.Add(curvesCount);
                foreach (ConversionTable ct in curves)
                    b0.AddRange(ct.Serialize());
                b0.AddRange(initialRTPC.Serialize());
                sectionSize = (uint)(b0.Count + 4);

                List<byte> b = new();
                b.AddRange(base.Serialize());
                b.AddRange(b0);
                return b;
            }
        }

        public class ConeParams : ISerializable
        {
            public float insideDegrees;
            public float outsideDegrees;
            public float outsideVolume;
            public float loPass;
            public float hiPass;

            public void Deserialize(WwiseData wd)
            {
                insideDegrees = ReadSingle(wd);
                outsideDegrees = ReadSingle(wd);
                outsideVolume = ReadSingle(wd);
                loPass = ReadSingle(wd);
                hiPass = ReadSingle(wd);
            }

            public IEnumerable<byte> Serialize()
            {
                List<byte> b = new();
                b.AddRange(GetBytes(insideDegrees));
                b.AddRange(GetBytes(outsideDegrees));
                b.AddRange(GetBytes(outsideVolume));
                b.AddRange(GetBytes(loPass));
                b.AddRange(GetBytes(hiPass));
                return b;
            }
        }

        public class ConversionTable : ISerializable
        {
            public byte scaling;
            public ushort size;
            public List<RTPCGraphPoint> pRTPCMgr;

            public void Deserialize(WwiseData wd)
            {
                pRTPCMgr = new();
                scaling = ReadUInt8(wd);
                size = ReadUInt16(wd);
                for (int i = 0; i < size; i++)
                {
                    RTPCGraphPoint rtpcgp = new();
                    rtpcgp.Deserialize(wd);
                    pRTPCMgr.Add(rtpcgp);
                }
            }

            public IEnumerable<byte> Serialize()
            {
                List<byte> b = new();
                b.Add(scaling);
                size = (ushort)pRTPCMgr.Count;
                b.AddRange(GetBytes(size));
                foreach (RTPCGraphPoint rtpcgp in pRTPCMgr)
                    b.AddRange(rtpcgp.Serialize());
                return b;
            }
        }

        public class RTPCGraphPoint : ISerializable, ICloneable
        {
            public float from;
            public float to;
            public uint interp;

            public void Deserialize(WwiseData wd)
            {
                from = ReadSingle(wd);
                to = ReadSingle(wd);
                interp = ReadUInt32(wd);
            }

            public IEnumerable<byte> Serialize()
            {
                List<byte> b = new();
                b.AddRange(GetBytes(from));
                b.AddRange(GetBytes(to));
                b.AddRange(GetBytes(interp));
                return b;
            }

            public object Clone()
            {
                return MemberwiseClone();
            }
        }

        public class AudioDevice : HircItem
        {
            public uint fxID;
            public uint size;
            public byte bankDataCount;
            public List<Unk> media;
            public InitialRTPC initialRTPC;
            public StateChunk stateChunk;
            public ushort valuesCount;
            public List<Unk> propertyValues;

            public override void Deserialize(WwiseData wd)
            {
                base.Deserialize(wd);

                media = new();
                initialRTPC = new();
                stateChunk = new();
                propertyValues = new();
                fxID = ReadUInt32(wd);
                size = ReadUInt32(wd);
                bankDataCount = ReadUInt8(wd);
                for (int i = 0; i < bankDataCount; i++)
                {
                    Unk u = new();
                    u.Deserialize(wd);
                    media.Add(u);
                }
                initialRTPC.Deserialize(wd);
                stateChunk.Deserialize(wd);
                valuesCount = ReadUInt16(wd);
                for (int i = 0; i < valuesCount; i++)
                {
                    Unk u = new();
                    u.Deserialize(wd);
                    propertyValues.Add(u);
                }
            }

            public override IEnumerable<byte> Serialize()
            {
                List<byte> b0 = new();
                b0.AddRange(GetBytes(fxID));
                b0.AddRange(GetBytes(size));
                bankDataCount = (byte)media.Count;
                b0.Add(bankDataCount);
                foreach (Unk u in media)
                    b0.AddRange(u.Serialize());
                b0.AddRange(initialRTPC.Serialize());
                b0.AddRange(stateChunk.Serialize());
                valuesCount = (ushort)propertyValues.Count;
                b0.AddRange(GetBytes(valuesCount));
                foreach (Unk u in propertyValues)
                    b0.AddRange(u.Serialize());
                sectionSize = (uint)(b0.Count + 4);

                List<byte> b = new();
                b.AddRange(base.Serialize());
                b.AddRange(b0);
                return b;
            }
        }

        public class PluginChunk : Chunk
        {
            public uint count;
            public List<Plugin> pluginList;

            public override void Deserialize(WwiseData wd)
            {
                base.Deserialize(wd);

                pluginList = new();
                count = ReadUInt32(wd);
                for (int i = 0; i < count; i++)
                {
                    Plugin p = new();
                    p.Deserialize(wd);
                    pluginList.Add(p);
                }
            }

            public override IEnumerable<byte> Serialize()
            {
                List<byte> b0 = new();
                count = (uint)pluginList.Count;
                b0.AddRange(GetBytes(count));
                foreach (Plugin p in pluginList)
                    b0.AddRange(p.Serialize());
                chunkSize = (uint)b0.Count;

                List<byte> b = new();
                b.AddRange(base.Serialize());
                b.AddRange(b0);
                return b;
            }
        }

        public class Plugin : ISerializable
        {
            public uint pluginID;
            public uint stringSize;
            public string dllName;

            public void Deserialize(WwiseData wd)
            {
                pluginID = ReadUInt32(wd);
                stringSize = ReadUInt32(wd);
                dllName = ReadString(wd, (int)stringSize);
            }

            public IEnumerable<byte> Serialize()
            {
                List<byte> b = new();
                b.AddRange(GetBytes(pluginID));
                b.AddRange(GetBytes(stringSize));
                b.AddRange(GetBytes(dllName));
                return b;
            }
        }

        public class CustomPlatformChunk : Chunk
        {
            public uint stringSize;
            public string customPlatformName;

            public override void Deserialize(WwiseData wd)
            {
                base.Deserialize(wd);

                stringSize = ReadUInt32(wd);
                customPlatformName = ReadString(wd, (int)stringSize);
            }

            public override IEnumerable<byte> Serialize()
            {
                List<byte> b0 = new();
                b0.AddRange(GetBytes(stringSize));
                b0.AddRange(GetBytes(customPlatformName));
                chunkSize = (uint)b0.Count;

                List<byte> b = new();
                b.AddRange(base.Serialize());
                b.AddRange(b0);
                return b;
            }
        }

        public class GlobalSettingsChunk : Chunk
        {
            public float volumeThreshold;
            public ushort maxNumVoicesLimitInternal;
            public ushort maxNumDangerousVirtVoicesLimitInternal;
            public uint stateGroupsCount;
            public List<StateGroup> stateGroups;
            public uint switchGroupsCount;
            public List<SwitchGroups> items;
            public uint paramsCount;
            public List<RTPCRamping> pRTPCMgr;
            public uint texturesCount;
            public List<Unk> acousticTextures;

            public override void Deserialize(WwiseData wd)
            {
                base.Deserialize(wd);

                stateGroups = new();
                items = new();
                pRTPCMgr = new();
                acousticTextures = new();
                volumeThreshold = ReadSingle(wd);
                maxNumVoicesLimitInternal = ReadUInt16(wd);
                maxNumDangerousVirtVoicesLimitInternal = ReadUInt16(wd);
                stateGroupsCount = ReadUInt32(wd);
                for (int i = 0; i < stateGroupsCount; i++)
                {
                    StateGroup sg = new();
                    sg.Deserialize(wd);
                    stateGroups.Add(sg);
                }
                switchGroupsCount = ReadUInt32(wd);
                for (int i = 0; i < switchGroupsCount; i++)
                {
                    SwitchGroups sg = new();
                    sg.Deserialize(wd);
                    items.Add(sg);
                }
                paramsCount = ReadUInt32(wd);
                for (int i = 0; i < paramsCount; i++)
                {
                    RTPCRamping rtpcr = new();
                    rtpcr.Deserialize(wd);
                    pRTPCMgr.Add(rtpcr);
                }
                texturesCount = ReadUInt32(wd);
                for (int i = 0; i < texturesCount; i++)
                {
                    Unk u = new();
                    u.Deserialize(wd);
                    acousticTextures.Add(u);
                }
            }

            public override IEnumerable<byte> Serialize()
            {
                List<byte> b0 = new();
                b0.AddRange(GetBytes(volumeThreshold));
                b0.AddRange(GetBytes(maxNumVoicesLimitInternal));
                b0.AddRange(GetBytes(maxNumDangerousVirtVoicesLimitInternal));
                stateGroupsCount = (uint)stateGroups.Count;
                b0.AddRange(GetBytes(stateGroupsCount));
                foreach (StateGroup sg in stateGroups)
                    b0.AddRange(sg.Serialize());
                switchGroupsCount = (uint)items.Count;
                b0.AddRange(GetBytes(switchGroupsCount));
                foreach (SwitchGroups sg in items)
                    b0.AddRange(sg.Serialize());
                paramsCount = (uint)pRTPCMgr.Count;
                b0.AddRange(GetBytes(paramsCount));
                foreach (RTPCRamping rtpcr in pRTPCMgr)
                    b0.AddRange(rtpcr.Serialize());
                texturesCount = (uint)acousticTextures.Count;
                b0.AddRange(GetBytes(texturesCount));
                foreach (Unk u in acousticTextures)
                    b0.AddRange(u.Serialize());
                chunkSize = (uint)b0.Count;

                List<byte> b = new();
                b.AddRange(base.Serialize());
                b.AddRange(b0);
                return b;
            }
        }

        public class StateGroup : WwiseObject
        {
            public uint stateGroupID;
            public uint defaultTransitionTime;
            public uint transitionsCount;
            public List<StateTransition> mapTransitions;

            public override void Deserialize(WwiseData wd)
            {
                mapTransitions = new();
                stateGroupID = ReadUInt32(wd);
                wd.objectsByID[stateGroupID] = this;
                defaultTransitionTime = ReadUInt32(wd);
                transitionsCount = ReadUInt32(wd);
                for (int i = 0; i < transitionsCount; i++)
                {
                    StateTransition st = new();
                    st.Deserialize(wd);
                    mapTransitions.Add(st);
                }
            }

            public override IEnumerable<byte> Serialize()
            {
                List<byte> b = new();
                b.AddRange(GetBytes(stateGroupID));
                b.AddRange(GetBytes(defaultTransitionTime));
                transitionsCount = (uint)mapTransitions.Count;
                b.AddRange(GetBytes(transitionsCount));
                foreach (StateTransition st in mapTransitions)
                    b.AddRange(st.Serialize());
                return b;
            }
        }

        public class StateTransition : ISerializable
        {
            public uint stateFrom;
            public uint stateTo;
            public uint transitionTime;

            public void Deserialize(WwiseData wd)
            {
                stateFrom = ReadUInt32(wd);
                stateTo = ReadUInt32(wd);
                transitionTime = ReadUInt32(wd);
            }

            public IEnumerable<byte> Serialize()
            {
                List<byte> b = new();
                b.AddRange(GetBytes(stateFrom));
                b.AddRange(GetBytes(stateTo));
                b.AddRange(GetBytes(transitionTime));
                return b;
            }
        }

        public class SwitchGroups : ISerializable
        {
            public uint switchGroupID;
            public uint rtpcID;
            public byte rtpcType;
            public uint size;
            public List<RTPCGraphPoint> switchMgr;

            public void Deserialize(WwiseData wd)
            {
                switchMgr = new();
                switchGroupID = ReadUInt32(wd);
                rtpcID = ReadUInt32(wd);
                rtpcType = ReadUInt8(wd);
                size = ReadUInt32(wd);
                for (int i = 0; i < size; i++)
                {
                    RTPCGraphPoint rtcpgp = new();
                    rtcpgp.Deserialize(wd);
                    switchMgr.Add(rtcpgp);
                }
            }

            public IEnumerable<byte> Serialize()
            {
                List<byte> b = new();
                b.AddRange(GetBytes(switchGroupID));
                b.AddRange(GetBytes(rtpcID));
                b.Add(rtpcType);
                size = (uint)switchMgr.Count;
                b.AddRange(GetBytes(size));
                foreach (RTPCGraphPoint rtcpgp in switchMgr)
                    b.AddRange(rtcpgp.Serialize());
                return b;
            }
        }

        public class RTPCRamping : WwiseObject
        {
            public uint rtpcID;
            public float value;
            public uint rampType;
            public float rampUp;
            public float rampDown;
            public byte bindToBuiltInParam;

            public override void Deserialize(WwiseData wd)
            {
                rtpcID = ReadUInt32(wd);
                wd.objectsByID[rtpcID] = this;
                value = ReadSingle(wd);
                rampType = ReadUInt32(wd);
                rampUp = ReadSingle(wd);
                rampDown = ReadSingle(wd);
                bindToBuiltInParam = ReadUInt8(wd);
            }

            public override IEnumerable<byte> Serialize()
            {
                List<byte> b = new();
                b.AddRange(GetBytes(rtpcID));
                b.AddRange(GetBytes(value));
                b.AddRange(GetBytes(rampType));
                b.AddRange(GetBytes(rampUp));
                b.AddRange(GetBytes(rampDown));
                b.Add(bindToBuiltInParam);
                return b;
            }
        }

        public class Unk : ISerializable
        {
            public void Deserialize(WwiseData wd)
            {
                throw new NotImplementedException();
            }

            public IEnumerable<byte> Serialize()
            {
                throw new NotImplementedException();
            }
        }
        public static uint FNV132(string data)
        {
            data = data.ToLower();
            uint hash = 0x811c9dc5;
            foreach (char c in data)
            {
                hash *= 0x01000193;
                hash ^= (byte)c;
            }
            return hash;
        }

        private static bool[] ReadFlags(WwiseData wd)
        {
            bool[] result = new bool[8];
            byte b = ReadUInt8(wd);
            for (int i = 0; i < 8; i++)
                result[i] = ((1 << i) & b) != 0;
            return result;
        }

        private static byte GetByte(bool[] data)
        {
            byte result = 0;
            for (int i = 0; i < data.Length; i++)
                result |= (byte)(data[i] ? 1 << i : 0);
            return result;
        }

        private static byte ReadUInt8(WwiseData wd)
        {
            byte result = wd.buffer[wd.offset];
            wd.offset++;
            return result;
        }

        private static ushort ReadUInt16(WwiseData wd)
        {
            ushort result = BitConverter.ToUInt16(wd.buffer, wd.offset);
            wd.offset += 2;
            return result;
        }

        private static IEnumerable<byte> GetBytes(ushort data)
        {
            return BitConverter.GetBytes(data);
        }

        private static short ReadInt16(WwiseData wd)
        {
            short result = BitConverter.ToInt16(wd.buffer, wd.offset);
            wd.offset += 2;
            return result;
        }

        private static IEnumerable<byte> GetBytes(short data)
        {
            return BitConverter.GetBytes(data);
        }

        private static string ReadString(WwiseData wd, int length)
        {
            string result = Encoding.ASCII.GetString(wd.buffer, wd.offset, length);
            wd.offset += length;
            return result;
        }

        private static IEnumerable<byte> GetBytes(string data)
        {
            return Encoding.ASCII.GetBytes(data);
        }

        private static uint ReadUInt32(WwiseData wd)
        {
            uint result = BitConverter.ToUInt32(wd.buffer, wd.offset);
            wd.offset += 4;
            return result;
        }

        private static IEnumerable<byte> GetBytes(int data)
        {
            return BitConverter.GetBytes(data);
        }

        private static int ReadInt32(WwiseData wd)
        {
            int result = BitConverter.ToInt32(wd.buffer, wd.offset);
            wd.offset += 4;
            return result;
        }

        private static IEnumerable<byte> GetBytes(uint data)
        {
            return BitConverter.GetBytes(data);
        }

        private static float ReadSingle(WwiseData wd)
        {
            float result = BitConverter.ToSingle(wd.buffer, wd.offset);
            wd.offset += 4;
            return result;
        }

        private static IEnumerable<byte> GetBytes(float data)
        {
            return BitConverter.GetBytes(data);
        }

        private static double ReadDouble(WwiseData wd)
        {
            double result = BitConverter.ToDouble(wd.buffer, wd.offset);
            wd.offset += 8;
            return result;
        }

        private static IEnumerable<byte> GetVariableIntBytes(ulong data)
        {
            List<byte> result = new();
            while (data > 0)
            {
                result.Add((byte)(data & 0x7F));
                data >>= 7;
            }
            if (result.Count == 0)
                result.Add(0);
            result.Reverse();
            for (int i = 0; i < result.Count - 1; i++)
                result[i] += 0x80;
            return result;
        }

        private static ulong ReadVariableInt(WwiseData wd)
        {
            ulong b = ReadUInt8(wd);
            ulong result = b & 0x7F;
            while ((b & 0x80) > 0)
            {
                b = ReadUInt8(wd);
                result = (result << 7) | (b & 0x7F);
            }

            return result;
        }

        private static IEnumerable<byte> GetBytes(double data)
        {
            return BitConverter.GetBytes(data);
        }

        private static byte[] ReadUInt8Array(WwiseData wd, int count)
        {
            byte[] result = new byte[count];
            Array.Copy(wd.buffer, wd.offset, result, 0, count);
            wd.offset += count;
            return result;
        }

        private static sbyte[] ReadInt8Array(WwiseData wd, int count)
        {
            sbyte[] result = new sbyte[count];
            for (int i = 0; i < count; i++)
                result[i] = (sbyte)wd.buffer[wd.offset + i];
            wd.offset += count;
            return result;
        }

        private static IEnumerable<byte> GetBytes(sbyte[] data)
        {
            return data.Select(b => (byte)b);
        }
    }
}
