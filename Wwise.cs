using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImpostersOrdeal
{
    /// <summary>
    /// Handles deserialization and serialization of the Wwise .bnk format.
    /// </summary>
    public static class Wwise
    {
        public class WwiseData
        {
            public byte[] buffer;
            public int offset;
            public readonly Dictionary<uint, WwiseObject> objectsByID;
            public Bank bank;

            public WwiseData(byte[] buffer)
            {
                this.buffer = buffer;
                offset = 0;
                objectsByID = new Dictionary<uint, WwiseObject>();
                bank = new Bank();
                bank.Deserialize(this);
            }

            public byte[] GetBytes()
            {
                return bank.Serialize().ToArray();
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
                    2 => new Sound(),
                    3 => Action.GetInstance(wd),
                    4 => new Event(),
                    5 => new RanSeqCntr(),
                    6 => new SwitchCntr(),
                    7 => new ActorMixer(),
                    9 => new LayerCntr(),
                    10 => new MusicSegment(),
                    11 => new MusicTrack(),
                    12 => new MusicSwitchCntr(),
                    13 => new MusicRanSeqCntr(),
                    14 => new Attenuation(),
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

        public class Bank : WwiseObject
        {
            public BankHeader bankHeader;
            public MediaIndex mediaIndex;
            public DataChunk dataChunk;
            public HircChunk hircChunk;

            public override void Deserialize(WwiseData wd)
            {
                bankHeader = new();
                mediaIndex = new();
                dataChunk = new();
                hircChunk = new();
                bankHeader.Deserialize(wd);
                wd.objectsByID[bankHeader.akBankHeader.soundBankID] = this;
                mediaIndex.Deserialize(wd);
                dataChunk.Deserialize(wd);
                hircChunk.Deserialize(wd);
            }

            public override IEnumerable<byte> Serialize()
            {
                List<byte> b = new();
                b.AddRange(bankHeader.Serialize());
                b.AddRange(mediaIndex.Serialize());
                b.AddRange(dataChunk.Serialize());
                b.AddRange(hircChunk.Serialize());
                return b;
            }
        }

        public class BankHeader : ISerializable
        {
            public string tag;
            public uint chunkSize;
            public AkBankHeader akBankHeader;

            public void Deserialize(WwiseData wd)
            {
                akBankHeader = new();
                tag = Read4Char(wd);
                chunkSize = ReadUInt32(wd);
                akBankHeader.Deserialize(wd);
            }

            public IEnumerable<byte> Serialize()
            {
                List<byte> b = new();
                b.AddRange(GetBytes(tag));
                b.AddRange(GetBytes(chunkSize));
                b.AddRange(akBankHeader.Serialize());
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
                wd.offset += 12;
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
                b.AddRange(new byte[12]);
                return b;
            }
        }

        public class MediaIndex : ISerializable
        {
            public string tag;
            public uint chunkSize;
            public List<MediaHeader> loadedMedia;

            public void Deserialize(WwiseData wd)
            {
                loadedMedia = new();
                tag = Read4Char(wd);
                chunkSize = ReadUInt32(wd);
                long endOffset = wd.offset + chunkSize;
                while (endOffset > wd.offset)
                {
                    MediaHeader mh = new();
                    mh.Deserialize(wd);
                    loadedMedia.Add(mh);
                }
            }

            public IEnumerable<byte> Serialize()
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

        public class DataChunk : ISerializable
        {
            public string tag;
            public uint chunkSize;
            public byte[] data;

            public void Deserialize(WwiseData wd)
            {
                tag = Read4Char(wd);
                chunkSize = ReadUInt32(wd);
                data = ReadUInt8Array(wd, (int)chunkSize);
            }

            public IEnumerable<byte> Serialize()
            {
                List<byte> b = new();
                b.AddRange(GetBytes(tag));
                b.AddRange(GetBytes(chunkSize));
                b.AddRange(data);
                return b;
            }
        }

        public class HircChunk : ISerializable
        {
            public string tag;
            public uint chunkSize;
            public uint releasableHircItemCount;
            public List<HircItem> loadedItem;

            public void Deserialize(WwiseData wd)
            {
                loadedItem = new();
                tag = Read4Char(wd);
                chunkSize = ReadUInt32(wd);
                releasableHircItemCount = ReadUInt32(wd);
                for (int i = 0; i < releasableHircItemCount; i++)
                    loadedItem.Add(HircItem.Create(wd));
            }

            public IEnumerable<byte> Serialize()
            {
                List<byte> b = new();
                b.AddRange(GetBytes(tag));
                b.AddRange(GetBytes(chunkSize));
                releasableHircItemCount = (uint)loadedItem.Count;
                b.AddRange(GetBytes(releasableHircItemCount));
                foreach (HircItem hircItem in loadedItem)
                    b.AddRange(hircItem.Serialize());
                return b;
            }
        }

        public class Sound : HircItem
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
                List<byte> b = new();
                b.AddRange(base.Serialize());
                b.AddRange(bankSourceData.Serialize());
                b.AddRange(nodeBaseParams.Serialize());
                return b;
            }
        }

        public class BankSourceData : ISerializable
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
        }

        public class MediaInformation : ISerializable
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
        }

        public class NodeBaseParams : ISerializable
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
        }

        public class NodeInitialFxParams : ISerializable
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
        }

        public class NodeInitialParams : ISerializable
        {
            public PropBundle0 propBundle0;
            public PropBundle2 propBundle1;

            public void Deserialize(WwiseData wd)
            {
                propBundle0 = new();
                propBundle1 = new();
                propBundle0.Deserialize(wd);
                propBundle1.Deserialize(wd);
            }

            public IEnumerable<byte> Serialize()
            {
                List<byte> b = new();
                b.AddRange(propBundle0.Serialize());
                b.AddRange(propBundle1.Serialize());
                return b;
            }
        }

        public class PropBundle0 : ISerializable
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
            }

            public IEnumerable<byte> Serialize()
            {
                List<byte> b = new();
                propsCount = (byte)props.Count;
                b.Add(propsCount);
                foreach (PropBundle1 pb1 in props)
                    b.AddRange(pb1.Serialize());
                return b;
            }
        }

        public class PropBundle1 : ISerializable
        {
            public byte id;
            public float value;

            public void Deserialize(WwiseData wd)
            {
                id = ReadUInt8(wd);
                value = ReadSingle(wd);
            }

            public IEnumerable<byte> Serialize()
            {
                List<byte> b = new();
                b.Add(id);
                b.AddRange(GetBytes(value));
                return b;
            }
        }

        public class PropBundle2 : ISerializable
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
        }

        public class PropBundle3 : ISerializable
        {
            public byte id;
            public float min;
            public float max;
            
            public void Deserialize(WwiseData wd)
            {
                id = ReadUInt8(wd);
            }

            public void DeserializeBoundaries(WwiseData wd)
            {
                min = ReadSingle(wd);
                max = ReadSingle(wd);
            }

            public IEnumerable<byte> Serialize()
            {
                return new byte[] { id };
            }

            public IEnumerable<byte> SerializeBoundaries()
            {
                List<byte> b = new();
                b.AddRange(GetBytes(min));
                b.AddRange(GetBytes(max));
                return b;
            }
        }

        public class PositioningParams : ISerializable
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
        }

        public class AuxParams : ISerializable
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
        }

        public class AdvSettingsParams : ISerializable
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
        }

        public class StateChunk : ISerializable
        {
            public byte statePropsCount;
            public List<Unk> stateProps;
            public byte stateGroupsCount;
            public List<Unk> stateChunks;

            public void Deserialize(WwiseData wd)
            {
                stateProps = new();
                stateChunks = new();
                statePropsCount = ReadUInt8(wd);
                for (int i = 0; i < statePropsCount; i++)
                {
                    Unk u = new();
                    u.Deserialize(wd);
                    stateProps.Add(u);
                }
                stateGroupsCount = ReadUInt8(wd);
                for (int i = 0; i < stateGroupsCount; i++)
                {
                    Unk u = new();
                    u.Deserialize(wd);
                    stateChunks.Add(u);
                }
            }

            public IEnumerable<byte> Serialize()
            {
                List<byte> b = new();
                statePropsCount = (byte)stateProps.Count;
                b.Add(statePropsCount);
                foreach (Unk u in stateProps)
                    b.AddRange(u.Serialize());
                stateGroupsCount = (byte)stateChunks.Count;
                b.Add(stateGroupsCount);
                foreach (Unk u in stateChunks)
                    b.AddRange(u.Serialize());
                return b;
            }
        }

        public class InitialRTPC : ISerializable
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
        }

        public class RTPC : WwiseObject
        {
            public uint rtpcID;
            public byte rtpcType;
            public byte rtpcAccum;
            public byte paramID;
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
                paramID = ReadUInt8(wd);
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
                b.Add(paramID);
                b.AddRange(GetBytes(rtpcCurveID));
                b.Add(scaling);
                size = (ushort)pRTPCMgr.Count;
                b.AddRange(GetBytes(size));
                foreach (RTPCGraphPoint rtpcgp in pRTPCMgr)
                    b.AddRange(rtpcgp.Serialize());
                return b;
            }
        }

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

        public class ActionPlay : Action
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
            public byte exceptionListSize;
            public List<WwiseObjectIDext> listElementException;

            public void Deserialize(WwiseData wd)
            {
                listElementException = new();
                exceptionListSize = ReadUInt8(wd);
                for (int i = 0; i < exceptionListSize; i++)
                {
                    WwiseObjectIDext woid = new();
                    woid.Deserialize(wd);
                    listElementException.Add(woid);
                }
            }

            public IEnumerable<byte> Serialize()
            {
                List<byte> b = new();
                exceptionListSize = (byte)listElementException.Count;
                b.Add(exceptionListSize);
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

        public class Event : HircItem
        {
            public byte actionListSize;
            public List<uint> actionIDs;

            public override void Deserialize(WwiseData wd)
            {
                base.Deserialize(wd);

                actionIDs = new();
                actionListSize = ReadUInt8(wd);
                for (int i = 0; i < actionListSize; i++)
                    actionIDs.Add(ReadUInt32(wd));
            }

            public override IEnumerable<byte> Serialize()
            {
                List<byte> b = new();
                b.AddRange(base.Serialize());
                actionListSize = (byte)actionIDs.Count;
                b.Add(actionListSize);
                foreach (uint i in actionIDs)
                    b.AddRange(GetBytes(i));
                return b;
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
                List<byte> b = new();
                b.AddRange(base.Serialize());
                b.AddRange(nodeBaseParams.Serialize());
                b.AddRange(GetBytes(loopCount));
                b.AddRange(GetBytes(loopModMin));
                b.AddRange(GetBytes(loopModMax));
                b.AddRange(GetBytes(transitionTime));
                b.AddRange(GetBytes(transitionTimeModMin));
                b.AddRange(GetBytes(transitionTimeModMax));
                b.AddRange(GetBytes(avoidRepeatCount));
                b.Add(transitionMode);
                b.Add(randomMode);
                b.Add(mode);
                bool[] flags = {
                    isUsingWeight,
                    resetPlayListAtEachPlay,
                    isRestartBackward,
                    isContinuous,
                    isGlobal
                };
                b.Add(GetByte(flags));
                b.AddRange(children.Serialize());
                b.AddRange(playList.Serialize());
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
                List<byte> b = new();
                b.AddRange(base.Serialize());
                b.AddRange(nodeBaseParams.Serialize());
                b.Add(groupType);
                b.AddRange(GetBytes(groupID));
                b.AddRange(GetBytes(defaultSwitch));
                b.Add(isContinuousValidation);
                b.AddRange(children.Serialize());
                switchGroupsCount = (uint)switchList.Count;
                b.AddRange(GetBytes(switchGroupsCount));
                foreach (SwitchPackage sp in switchList)
                    b.AddRange(sp.Serialize());
                switchParamsCount = (uint)paramList.Count;
                b.AddRange(GetBytes(switchParamsCount));
                foreach (SwitchNodeParams snp in paramList)
                    b.AddRange(snp.Serialize());
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

        public class ActorMixer : HircItem
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
                List<byte> b = new();
                b.AddRange(base.Serialize());
                b.AddRange(nodeBaseParams.Serialize());
                b.AddRange(children.Serialize());
                return b;
            }
        }

        public class Children : ISerializable
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
                List<byte> b = new();
                childsCount = (uint)childIDs.Count;
                b.AddRange(GetBytes(childsCount));
                foreach (uint i in childIDs)
                    b.AddRange(GetBytes(i));
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
                List<byte> b = new();
                b.AddRange(base.Serialize());
                b.AddRange(nodeBaseParams.Serialize());
                b.AddRange(children.Serialize());
                layersCount = (uint)layers.Count;
                b.AddRange(GetBytes(layersCount));
                foreach (Layer l in layers)
                    b.AddRange(l.Serialize());
                b.Add(isContinuousValidation);
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
                List<byte> b = new();
                b.AddRange(base.Serialize());
                b.AddRange(musicNodeParams.Serialize());
                b.AddRange(GetBytes(duration));
                markersCount = (uint)arrayMarkers.Count;
                b.AddRange(GetBytes(markersCount));
                foreach (MusicMarkerWwise mmw in arrayMarkers)
                    b.AddRange(mmw.Serialize());
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
                List<byte> b = new(); 
                b.AddRange(base.Serialize());
                bool[] flags = { false,
                    overrideParentMidiTempo,
                    overrideParentMidiTarget,
                    midiTargetTypeBus
                };
                b.Add(GetByte(flags));
                sourceCount = (uint)source.Count;
                b.AddRange(GetBytes(sourceCount));
                foreach (BankSourceData bsd in source)
                    b.AddRange(bsd.Serialize());
                playlistItemCount = (uint)playlist.Count;
                b.AddRange(GetBytes(playlistItemCount));
                foreach (TrackSrcInfo tsi in playlist)
                    b.AddRange(tsi.Serialize());
                b.AddRange(GetBytes(subTrackCount));
                clipAutomationItemCount = (uint)items.Count;
                b.AddRange(GetBytes(clipAutomationItemCount));
                foreach (ClipAutomation ca in items)
                    b.AddRange(ca.Serialize());
                b.AddRange(nodeBaseParams.Serialize());
                b.Add(trackType);
                if (switchParams != null)
                    b.AddRange(switchParams.Serialize());
                if (transParams != null)
                    b.AddRange(transParams.Serialize());
                b.AddRange(GetBytes(lookAheadTime));
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
                List<byte> b = new();
                b.AddRange(base.Serialize());
                b.AddRange(musicTransNodeParams.Serialize());
                b.Add(isContinuePlayback);
                b.AddRange(GetBytes(treeDepth));
                foreach (GameSync gs in arguments)
                    b.AddRange(gs.Serialize());
                foreach (GameSync gs in arguments)
                    b.AddRange(gs.SerializeGroupType());
                b.AddRange(GetBytes(treeDataSize));
                b.Add(mode);
                b.AddRange(decisionTree.Serialize());
                b.AddRange(decisionTree.SerializeChildren());
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

            private readonly int level;

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
                List<byte> b = new();
                b.AddRange(GetBytes(key));
                if (level > 0)
                {
                    b.AddRange(GetBytes(childrenIdx));
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
                foreach (Node n in nodes)
                    b.AddRange(n.Serialize());
                foreach (Node n in nodes)
                    b.AddRange(n.SerializeChildren());
                return b;
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
                List<byte> b = new();
                b.AddRange(base.Serialize());
                b.AddRange(musicTransNodeParams.Serialize());
                playlistItemsCount = 0;
                foreach (MusicRanSeqPlaylistItem mrspi in playList)
                    playlistItemsCount += 1 + (uint)mrspi.GetChildrenCount();
                b.AddRange(GetBytes(playlistItemsCount));
                foreach (MusicRanSeqPlaylistItem mrspi in playList)
                    b.AddRange(mrspi.Serialize());
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
                List<byte> b = new();
                b.AddRange(base.Serialize());
                isConeEnabled = (byte)(coneParams == null ? 0 : 1);
                b.Add(isConeEnabled);
                if (coneParams != null)
                    b.AddRange(coneParams.Serialize());
                b.AddRange(GetBytes(curveToUse));
                curvesCount = (byte)curves.Count;
                b.Add(curvesCount);
                foreach (ConversionTable ct in curves)
                    b.AddRange(ct.Serialize());
                b.AddRange(initialRTPC.Serialize());
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

        public class RTPCGraphPoint : ISerializable
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

        private static string Read4Char(WwiseData wd)
        {
            string result = Encoding.ASCII.GetString(wd.buffer, wd.offset, 4);
            wd.offset += 4;
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
