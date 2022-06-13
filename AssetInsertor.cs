using AssetsTools.NET;
using AssetsTools.NET.Extra;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace ImpostersOrdeal
{
    class AssetInsertor
    {
        enum AssetClassID
        {
            GameObject = 1,
            Texture2D = 28,
            Mesh = 43,
            AnimationClip = 74,
            AssetBundle = 142,
        }

        private const String basePath = "\\Pokemon Database\\pokemons\\";

        private static AssetInsertor instance;
        private Random rnd;
        private Dictionary<String, String> CABNames;
        private AssetInsertor()
        {
            rnd = new();
            CABNames = new();
        }
        public static AssetInsertor getInstance()
        {
            if (instance == null)
            {
                instance = new();
            }
            return instance;
        }
        public string genCABName()
        {
            string[] values = new string[4];
            for (int i = 0; i < values.Length; i++)
            {
                values[i] = rnd.Next().ToString("x2");
            }

            return String.Join("", values);
        }

        public void InsertPokemon(int baseMonsNo, int monsNo, int baseFormNo, int formNo, string formName)
        {
            DuplicateAssetBundles(baseMonsNo, monsNo, baseFormNo, formNo);
        }

        public void DuplicateAssetBundles(int baseMonsNo, int monsNo, int baseFormNo, int formNo)
        {
            String commonPath = string.Format("common\\pm{0}_{1}", baseMonsNo.ToString("D4"), baseFormNo.ToString("D2"));
            String common00Path = string.Format("common\\pm{0}_{1}_00", baseMonsNo.ToString("D4"), baseFormNo.ToString("D2"));
            String common01Path = string.Format("common\\pm{0}_{1}_01", baseMonsNo.ToString("D4"), baseFormNo.ToString("D2"));
            String battle00Path = string.Format("battle\\pm{0}_{1}_00", baseMonsNo.ToString("D4"), baseFormNo.ToString("D2"));
            String battle01Path = string.Format("battle\\pm{0}_{1}_01", baseMonsNo.ToString("D4"), baseFormNo.ToString("D2"));
            String field00Path = string.Format("field\\pm{0}_{1}_00", baseMonsNo.ToString("D4"), baseFormNo.ToString("D2"));
            String field01Path = string.Format("field\\pm{0}_{1}_01", baseMonsNo.ToString("D4"), baseFormNo.ToString("D2"));
            BundleFileInstance commonBfi = DuplicateAssetBundle(commonPath, baseMonsNo, monsNo, baseFormNo, formNo);
            BundleFileInstance common00Bfi = DuplicateAssetBundle(common00Path, baseMonsNo, monsNo, baseFormNo, formNo);
            BundleFileInstance common01Bfi = DuplicateAssetBundle(common01Path, baseMonsNo, monsNo, baseFormNo, formNo);
            BundleFileInstance battle00Bfi = DuplicateAssetBundle(battle00Path, baseMonsNo, monsNo, baseFormNo, formNo);
            BundleFileInstance battle01Bfi = DuplicateAssetBundle(battle01Path, baseMonsNo, monsNo, baseFormNo, formNo);
            BundleFileInstance field00Bfi = DuplicateAssetBundle(field00Path, baseMonsNo, monsNo, baseFormNo, formNo);
            BundleFileInstance field01Bfi = DuplicateAssetBundle(field01Path, baseMonsNo, monsNo, baseFormNo, formNo);

            String newCommonPath = string.Format("common_pm{0}_{1}", monsNo.ToString("D4"), formNo.ToString("D2"));
            String newCommon00Path = string.Format("common_pm{0}_{1}_00", monsNo.ToString("D4"), formNo.ToString("D2"));
            String newCommon01Path = string.Format("common_pm{0}_{1}_01", monsNo.ToString("D4"), formNo.ToString("D2"));
            String newBattle00Path = string.Format("battle_pm{0}_{1}_00", monsNo.ToString("D4"), formNo.ToString("D2"));
            String newBattle01Path = string.Format("battle_pm{0}_{1}_01", monsNo.ToString("D4"), formNo.ToString("D2"));
            String newField00Path = string.Format("field_pm{0}_{1}_00", monsNo.ToString("D4"), formNo.ToString("D2"));
            String newField01Path = string.Format("field_pm{0}_{1}_01", monsNo.ToString("D4"), formNo.ToString("D2"));
            writeBundle(commonBfi, newCommonPath);
            writeBundle(common00Bfi, newCommon00Path);
            writeBundle(common01Bfi, newCommon01Path);
            writeBundle(battle00Bfi, newBattle00Path);
            writeBundle(battle01Bfi, newBattle01Path);
            writeBundle(field00Bfi, newField00Path);
            writeBundle(field01Bfi, newField01Path);
        }

        public void writeBundle(BundleFileInstance bfi, string ofpath)
        {
            FileStream stream = File.OpenWrite(ofpath);
            AssetsFileWriter afw = new AssetsFileWriter(stream);
            bfi.file.Pack(bfi.file.reader, afw, AssetBundleCompressionType.LZ4);
            afw.Close();
            bfi.file.Close();
            bfi.stream.Dispose();
        }

        public BundleFileInstance DuplicateAssetBundle(String path, int baseMonsNo, int monsNo, int baseFormNo, int formNo)
        {
            String newCAB = genCABName();
            String oldPMName = string.Format("pm{0}_{1}", baseMonsNo.ToString("D4"), baseFormNo.ToString("D2"));
            String newPMName = string.Format("pm{0}_{1}", monsNo.ToString("D4"), formNo.ToString("D2"));
            AssetsManager am = GlobalData.fileManager.getAssetsManager();
            BundleFileInstance bfi = GlobalData.fileManager.GetBundleFileInstance(basePath + path);
            bfi.name = bfi.name.Replace(oldPMName, newPMName);

            AssetsFileInstance afi = am.LoadAssetsFileFromBundle(bfi, 0);
            String oldCAB = afi.name.Replace("CAB-", "");
            afi.name = afi.name.Replace(oldCAB, newCAB);
            CABNames.Add(oldCAB, newCAB);

            // Just need to make sure I do things in the right order, handle dependencies before
            // the bundles that depend on them.
            for (int i = 0; i < afi.dependencies.Count; i++)
            {
                AssetsFileInstance dependency = afi.dependencies[i];
                if (dependency == null)
                {
                    continue;
                }

                String dependencyName = dependency.name;
                foreach (String cabName in CABNames.Keys)
                {
                    dependencyName.Replace(cabName, CABNames[cabName]);
                }
                afi.dependencies[i].name = dependencyName;
            }

            List<AssetTypeValueField> gameObjects = afi.table.GetAssetsOfType((int)AssetClassID.GameObject).Select(afie => am.GetTypeInstance(afi, afie).GetBaseField()).ToList();
            AssetTypeValueField gameObject;

            for (int i = 0; i < gameObjects.Count; i++)
            {
                gameObject = gameObjects[i];

                string m_Name = gameObject["m_Name"].value.AsString();
                m_Name = m_Name.Replace(oldPMName, newPMName);
                gameObject["m_Name"].GetValue().Set(m_Name);
            }

            List<AssetTypeValueField> texture2Ds = afi.table.GetAssetsOfType((int)AssetClassID.Texture2D).Select(afie => am.GetTypeInstance(afi, afie).GetBaseField()).ToList();

            AssetTypeValueField texture2DField;
            for (int i = 0; i < texture2Ds.Count; i++)
            {
                texture2DField = texture2Ds[i];

                string m_Name = texture2DField["m_Name"].value.AsString();
                String m_StreamDataPath = texture2DField["m_StreamData"].children[2].value.AsString();
                m_Name = m_Name.Replace(oldPMName, newPMName);
                m_StreamDataPath = m_StreamDataPath.Replace(oldCAB, newCAB);
                texture2DField["m_Name"].GetValue().Set(m_Name);
                texture2DField["m_StreamData"].children[2].GetValue().Set(m_StreamDataPath);
            }

            List<AssetTypeValueField> meshes = afi.table.GetAssetsOfType((int)AssetClassID.GameObject).Select(afie => am.GetTypeInstance(afi, afie).GetBaseField()).ToList();
            AssetTypeValueField mesh;

            for (int i = 0; i < meshes.Count; i++)
            {
                mesh = meshes[i];

                string m_Name = mesh["m_Name"].value.AsString();
                m_Name = m_Name.Replace(oldPMName, newPMName);
                mesh["m_Name"].GetValue().Set(m_Name);
            }


            List<AssetTypeValueField> animationClips = afi.table.GetAssetsOfType((int)AssetClassID.AnimationClip).Select(afie => am.GetTypeInstance(afi, afie).GetBaseField()).ToList();

            AssetTypeValueField animationClip;
            for (int i = 0; i < animationClips.Count; i++)
            {
                animationClip = animationClips[i];

                string m_Name = animationClip["m_Name"].value.AsString();
                m_Name = m_Name.Replace(oldPMName, newPMName);
                animationClip["m_Name"].GetValue().Set(m_Name);
            }

            List<AssetTypeValueField> assetBundles = afi.table.GetAssetsOfType((int)AssetClassID.AssetBundle).Select(afie => am.GetTypeInstance(afi, afie).GetBaseField()).ToList();
            AssetTypeValueField assetBundle;
            for (int i = 0; i < assetBundles.Count; i++)
            {
                assetBundle = assetBundles[i];

                string m_Name = assetBundle["m_Name"].value.AsString();
                m_Name = m_Name.Replace(oldPMName, newPMName);
                assetBundle["m_Name"].GetValue().Set(m_Name);

                AssetTypeValueField m_Dependencies = assetBundle["m_Dependencies"][0];
                for (int j = 0; j < m_Dependencies.childrenCount; j++)
                {
                    AssetTypeValueField entry = m_Dependencies.children[j];
                    string key = entry.value.AsString();
                    key = key.Replace(oldPMName, newPMName);
                    entry.GetValue().Set(key);
                }

                AssetTypeValueField m_Container = assetBundle["m_Container"][0];

                for (int j = 0; j < m_Container.childrenCount; j++)
                {
                    AssetTypeValueField entry = m_Container.children[j];
                    string key = entry.children[0].value.AsString();
                    key = key.Replace(oldPMName, newPMName);
                    entry.children[0].GetValue().Set(key);
                }
            }

            return bfi;
        }
    }
}