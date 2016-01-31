using System;
using System.Collections;
using Tekla.Structures.Model;

namespace SKA_DS_profile_check
{
    internal class ObjectInfo
    {
        public const string TplPartName = "NAME";
        public const string TplPartMaterial = "MATERIAL";
        public const string TplPartProfile = "PROFILE";
        public const string TplPartClass = "CLASS_ATTR";
        public const string TplPartPrefix = "PART_PREFIX";
        public const string TplPartMark = "PART_POS";
        public const string TplPartAssemblyPrefix = "ASSEMBLY.ASSEMBLY_PREFIX";
        public const string TplPartAssemblyMark = "ASSEMBLY.ASSEMBLY_POS";
        public const string TplPartStartNo = "PART_STARTNO";
        public const string TplPartAssemblyStartNo = "ASSEMBLY.ASSEMBLY_STARTNO";
        private static ArrayList ObjectStringProperties = new ArrayList(new string[]
        {
            "ASSEMBLY.ASSEMBLY_POS",
            "ASSEMBLY.ASSEMBLY_PREFIX",
            "CLASS_ATTR",
            "PART_POS",
            "MATERIAL",
            "NAME",
            "PART_PREFIX",
            "PROFILE"
        });
        private static ArrayList ObjectIntegerProperties = new ArrayList(new string[]
        {
            "ASSEMBLY.ASSEMBLY_STARTNO",
            "PART_STARTNO"
        });
        private int _objectID;
        private string _partGUID;
        private string _partMark;
        private string _partPrefix;
        private int _partStNr;
        private string _assemblyPrefix;
        private int _assemblyStNr;
        private string _partName;
        private string _partMaterial;
        private string _partProfile;
        private string _partClass;
        public int ObjectID
        {
            get
            {
                return this._objectID;
            }
            set
            {
                this._objectID = value;
            }
        }
        public string PartGUID
        {
            get
            {
                return this._partGUID;
            }
            set
            {
                this._partGUID = value;
            }
        }
        public string PartMark
        {
            get
            {
                return this._partMark;
            }
            set
            {
                this._partMark = value;
            }
        }
        public string PartPrefix
        {
            get
            {
                return this._partPrefix;
            }
            set
            {
                this._partPrefix = value;
            }
        }
        public int PartStNr
        {
            get
            {
                return this._partStNr;
            }
            set
            {
                this._partStNr = value;
            }
        }
        public string AssemblyPrefix
        {
            get
            {
                return this._assemblyPrefix;
            }
            set
            {
                this._assemblyPrefix = value;
            }
        }
        public int AssemblyStNr
        {
            get
            {
                return this._assemblyStNr;
            }
            set
            {
                this._assemblyStNr = value;
            }
        }
        public string PartName
        {
            get
            {
                return this._partName;
            }
            set
            {
                this._partName = value;
            }
        }
        public string PartMaterial
        {
            get
            {
                return this._partMaterial;
            }
            set
            {
                this._partMaterial = value;
            }
        }
        public string PartProfile
        {
            get
            {
                return this._partProfile;
            }
            set
            {
                this._partProfile = value;
            }
        }
        public string PartClass
        {
            get
            {
                return this._partClass;
            }
            set
            {
                this._partClass = value;
            }
        }
        public ObjectInfo(ModelObject modelObject)
        {
            Model model = new Model();
            Hashtable hashtable = new Hashtable();
            Hashtable hashtable2 = new Hashtable();
            modelObject.GetStringReportProperties(ObjectInfo.ObjectStringProperties, ref hashtable);
            modelObject.GetIntegerReportProperties(ObjectInfo.ObjectIntegerProperties, ref hashtable2);
            this._objectID = modelObject.Identifier.ID;
            this._partGUID = model.GetGUIDByIdentifier(modelObject.Identifier);
            this._partStNr = 0;
            this._assemblyStNr = 0;
            this._partMark = (string)hashtable["PART_POS"];
            this._partPrefix = (string)hashtable["PART_PREFIX"];
            this._assemblyPrefix = (string)hashtable["ASSEMBLY.ASSEMBLY_PREFIX"];
            this._partName = (string)hashtable["NAME"];
            this._partMaterial = (string)hashtable["MATERIAL"];
            this._partProfile = (string)hashtable["PROFILE"];
            this._partClass = (string)hashtable["CLASS_ATTR"];
        }
        public ObjectInfo(int objectID, string guid, string partMark, string partPrefix, int partStNr, string assemblyPrefix, int assemblyStNr, string partName, string partMaterial, string partProfile, string partClass)
        {
            this._objectID = objectID;
            this._partGUID = guid;
            this._partMark = partMark;
            this._partPrefix = partPrefix;
            this._partStNr = partStNr;
            this._assemblyPrefix = assemblyPrefix;
            this._assemblyStNr = assemblyStNr;
            this._partName = partName;
            this._partMaterial = partMaterial;
            this._partProfile = partProfile;
            this._partClass = partClass;
        }
    }
}

