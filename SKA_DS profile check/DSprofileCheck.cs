using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tekla.Structures.Model;
using Tekla.Structures.Model.Operations;
using Tekla.Structures.Model.UI;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace SKA_DS_profile_check
{
   internal class DSprofileCheck
    {
        //internal Settings settings;
        //internal DSprofileCheck(ref Settings settingsIn)
        //{
        //    this.settings = settingsIn;
        //}
        public bool CheckProjectMaterials(bool checkAll, ref List<ObjectInfo> InvalidObjectList, ref int foundPartCount, ref int ignoredPartCount)
        {
            InvalidObjectList.Clear();
            List<ModelObjectEnumerator> objectsToCheck = this.GetObjectsToCheck(checkAll);
            foreach (ModelObjectEnumerator current in objectsToCheck)
            {
                this.CheckObjectList(ref InvalidObjectList, current, ref foundPartCount, ref ignoredPartCount);
            }
            return InvalidObjectList.Count <= 0;
        }
        private List<ModelObjectEnumerator> GetObjectsToCheck(bool checkAll)
        {
            Model model = new Model();
            List<ModelObjectEnumerator> list = new List<ModelObjectEnumerator>();
            if (checkAll)
            {
                list.Add(model.GetModelObjectSelector().GetAllObjectsWithType(ModelObject.ModelObjectEnum.BEAM));
                list.Add(model.GetModelObjectSelector().GetAllObjectsWithType(ModelObject.ModelObjectEnum.CONTOURPLATE));
                list.Add(model.GetModelObjectSelector().GetAllObjectsWithType(ModelObject.ModelObjectEnum.POLYBEAM));
            }
            else
            {
                Tekla.Structures.Model.UI.ModelObjectSelector modelObjectSelector = new Tekla.Structures.Model.UI.ModelObjectSelector();
                ModelObjectEnumerator selectedObjects = modelObjectSelector.GetSelectedObjects();
                list.Add(selectedObjects);
            }
            return list;
        }
        private void CheckObjectList(ref List<ObjectInfo> problemObjects, ModelObjectEnumerator partList, ref int partFoundCount, ref int ignorePartCount)
        {
            partList.SelectInstances = false;

            Tekla.Structures.Model.Model model = new Model();
            if (!model.GetConnectionStatus())
            {
                MessageBox.Show("Tekla is not open.");
                Environment.Exit(1);
            }

            foreach (ModelObject modelObject in partList)
            {
                if (true) // !this.IgnoreObject(modelObject))
                {
                    Part part = modelObject as Part;
                    if (part != null)
                    {
                        partFoundCount++;

                        string profile = "";
                        string profileType = "";
                        double height = new double();
                        double width = new double();
                        double wallThickness = new double();

                        string[] profileList;

                        part.GetReportProperty("PROFILE", ref profile);
                        part.GetReportProperty("PROFILE_TYPE", ref profileType);
                        part.GetReportProperty("HEIGHT", ref height);
                        part.GetReportProperty("WIDTH", ref width);
                        part.GetReportProperty("PROFILE.PLATE_THICKNESS", ref wallThickness);
                        
                        // splits profile string into segments, uses numerical values as split separators
                        profileList = Regex.Split(profile, @"[0-9]+");
                        
                        MessageBox.Show(
                            "profile:\n" + 
                            profile + 
                            "\n profile Type: \n" +
                            profileType + 
                            "\n profileList:\n" + 
                            profileList[0] +
                            "\n height:" + height +
                            "\n width:" + width);

                        // round dimensions to 0 or 1 digits
                        height = DimensionRound(height);
                        width = DimensionRound(width);
                        wallThickness = DimensionRound(wallThickness);

                        if (profileType == "M" && profileList != null) // M for rectangular tubes
                        {
                            if (profileList.Length >= 2)
                            {
                                if (profileList[0] != "RHS" || profileList[1] != "X")
                                {
                                    // SHS profiles dont' have parameter 'width'
                                    if (width == 0) width = height;

                                    part.Select();
                                    // System.Globalization.CultureInfo.InvariantCulture is used, to always get dots (and not commas) in cases,
                                    // where one of profile parameters is rounded to 1 digit (e.g. 88.9 instead of 88,9)
                                    part.Profile.ProfileString = 
                                        "RHS" + 
                                        height.ToString(System.Globalization.CultureInfo.InvariantCulture) + 
                                        "X" + 
                                        width.ToString(System.Globalization.CultureInfo.InvariantCulture) + 
                                        "X" + 
                                        wallThickness;
                                    try
                                    {
                                        part.Modify();
                                    }
                                    catch (Exception e)
                                    {
                                        MessageBox.Show(e.ToString());
                                    }
                                }
                            }
                        }

                        if (profileType == "RO") // RO for circular tubes
                        {
                            if (profileList.Length >= 2)
                            {
                                if (profileList[0] != "Ø" || profileList[1] != "X")
                                {
                                    part.Select();

                                    part.Profile.ProfileString = 
                                        "Ø" + 
                                        height.ToString(System.Globalization.CultureInfo.InvariantCulture) + 
                                        "X" + 
                                        wallThickness.ToString(System.Globalization.CultureInfo.InvariantCulture);

                                    part.Modify();
                                }
                            }
                        }

                        if (profileType == "RU") // RU for circular rods
                        {
                            if (profileList.Length >= 1)
                            {
                                if (profileList[0] != "D")
                                {
                                    part.Profile.ProfileString = "D" + height.ToString(System.Globalization.CultureInfo.InvariantCulture);
                                    part.Modify();
                                }
                            }
                        }

                        /*
                        if (!(this.settings.PropertyName == "AssemblyPrefix") || IsMainPart(part))
                        {
                            string item = "asdfghjkl9999";
                            if (!part.GetReportProperty(this.settings.PropertyReportField, ref item) || !this.settings.ProjectProperties.Contains(item))
                            {
                                problemObjects.Add(new ObjectInfo(part));
                            }
                        }
                        */
                    }
                }
                else
                {
                    ignorePartCount++;
                }
            }
        }

        private static double DimensionRound(double input)
        {
            if (10 * Math.Floor(input) == Math.Floor(10 * input)) input = Math.Round(input, 0);
            else input = Math.Round(input, 1);
            return input;
        }

        private static bool IsMainPart(Part part)
        {
            int num = 0;
            part.GetReportProperty("MAIN_PART", ref num);
            return num == 1;
        }
        /*
        private bool IgnoreObject(ModelObject modelObject)
        {
            Model model = new Model();
            string gUIDByIdentifier = model.GetGUIDByIdentifier(modelObject.Identifier);
            bool result = false;
            if (this.settings.ObjectsToIgnore.Contains(gUIDByIdentifier))
            {
                result = true;
            }
            else
            {
                int count = this.settings.FilterList.Count;
                for (int i = 0; i < count; i++)
                {
                    string filterName = this.settings.FilterList[i];
                    if (Operation.ObjectMatchesToFilter(modelObject, filterName))
                    {
                        result = true;
                    }
                }
            }
            return result;
        }*/
        private bool CheckIfIgnored(ModelObjectEnumerator modelObjectEnumerator, ModelObject part)
        {
            bool result = false;
            if (modelObjectEnumerator != null)
            {
                foreach (ModelObject modelObject in modelObjectEnumerator)
                {
                    if (part.Identifier.ID == modelObject.Identifier.ID)
                    {
                        return true;
                    }
                }
                return result;
            }
            return result;
        }
    }
}
