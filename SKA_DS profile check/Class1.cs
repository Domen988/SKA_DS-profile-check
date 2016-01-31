using ModelChecker.PluginInterface;
using System;
using System.Collections.Generic;
using System.Drawing;
using Tekla.Structures;

namespace SKA_DS_profile_check
{
    public class DS_profile_check : ITeklaStructuresCheck
    {
        internal static string SettingsDialogTitle = "SKA_DS profile check";
        internal static string GroupBoxTitle = "albl_Profile_list";
        internal static string ResolutionDialogTitle = "albl_Invalid_part_profiles";
        //internal static Settings settings;
        //internal static ResolutionForm resolvePlugin1;
        internal static List<ObjectInfo> invalidObjectList = new List<ObjectInfo>();
        //internal static SettingsForm settingsForm;
        internal static DSprofileCheck dsProfileCheck;
        internal static string HelpText = "albl_Checks_that_all_the_parts_has_valid_profiles";
        internal static string HelpName = "ProjectProfile";

        //Thumbnail must be approximately 38x38 px square
        internal Bitmap Thumbnail = new Bitmap(SKA_DS_profile_check.Properties.Resources.SKA_DS_profile_check);
        string ITeklaStructuresCheck.Name
        {
            get
            {
                return SettingsDialogTitle;
            }
        }
        string ITeklaStructuresCheck.ToolTipHelp
        {
            get
            {
                return "Checks and replaces all non-DS profile strings.";
            }
        }
        Bitmap ITeklaStructuresCheck.Thumbnail
        {
            get
            {
                return this.Thumbnail;
            }
        }
        bool ITeklaStructuresCheck.HasSettings
        {
            get
            {
                return false;
            }
        }
        bool ITeklaStructuresCheck.HasResolutionTool
        {
            get
            {
                return false;
            }
        }
        public DS_profile_check()
        {
            // DS_profile_check.settings = new Settings("PROFILE", "PartProfile");
            // DS_profile_check.settings.grupBoxName = DS_profile_check.GroupBoxTitle;
            // DS_profile_check.settings.titleName = DS_profile_check.SettingsDialogTitle;
            // DS_profile_check.settings.HelpName = DS_profile_check.HelpName;
            // DS_profile_check.settingsForm = new PartProfile(ref DS_profile_check.settings);
            DS_profile_check.dsProfileCheck = new DSprofileCheck();//ref DS_profile_check.settings);
            this.SetThumbnail();
        }
        void ITeklaStructuresCheck.ShowSettings()
        {
            //DS_profile_check.settingsForm.ShowDialog();
            //DS_profile_check.settingsForm.Focus();
        }
        bool ITeklaStructuresCheck.CheckModel(bool all)
        {
            int foundPartCount = 0;
            int ignoredPartCount = 0;
            bool result = DS_profile_check.dsProfileCheck.CheckProjectMaterials(all, ref DS_profile_check.invalidObjectList, ref foundPartCount, ref ignoredPartCount);
            //DS_profile_check.resolvePlugin1 = new ResolutionForm(foundPartCount, ignoredPartCount, ref MyPlugin2.settingsForm, ref MyPlugin2.invalidObjectList);
            //DS_profile_check.resolvePlugin1.Text = DS_profile_check.ResolutionDialogTitle;
            return result;
        }
        void ITeklaStructuresCheck.ShowResolutionTool()
        {
            //MyPlugin2.resolvePlugin1.ShowDialog();
            //MyPlugin2.resolvePlugin1.Focus();
        }
        private void SetThumbnail()
        {
            try
            {
                this.Thumbnail = new Bitmap(SKA_DS_profile_check.Properties.Resources.SKA_DS_profile_check);
            }
            catch
            {
            }
        }
    }
}
