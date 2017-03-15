using Microsoft.EnterpriseManagement.GenericForm;
using Microsoft.EnterpriseManagement.UI.Extensions.Shared;
using Microsoft.EnterpriseManagement.UI.WpfControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Custom.SM.IR.UserControlOverride
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class Override : UserControl
    {
        private bool _init = false;
        public Override()
        {
            InitializeComponent();
        }

        private DependencyObject GetParentDependancyObject(DependencyObject child, string name)
        {
            try
            {
                //We need the logical tree to get our parent
                DependencyObject parent = LogicalTreeHelper.GetParent(child);
                DependencyObject lastparent = null;
                //Is the parent our specified control?
                if (name != "" && parent.GetType().ToString() == name) return parent;
                //No, process further
                while (parent != null)
                {
                    string s = parent.GetType().ToString();
                    if (s == name && name != "") return parent;
                    parent = LogicalTreeHelper.GetParent(parent);
                    if (parent != null) lastparent = parent;
                }
                //Return results
                if (name != "") return null;
                else return lastparent;
            }
            catch
            {
                return null;
            }
        }

        private void UserControl_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (!_init)
            {
                if (!ConsoleContextHelper.Instance.UserRoleHelper.IsUserAdministrator &&
                    !FormUtilities.Instance.IsFormInTemplateMode(Window.GetWindow(this)))
                {
                    //Get tab item
                    DependencyObject doTabControl = GetParentDependancyObject(this, "System.Windows.Controls.TabControl");
                    TabControl tc = (TabControl) doTabControl;
                    TabItem tabitem_general = (TabItem) tc.FindName("TabItem_General");


                    UserPicker userPicker = (UserPicker) tabitem_general.FindName("AssignedTo");
                    userPicker.IsReadOnly = true;
                    ListPicker listPicker = (ListPicker) tabitem_general.FindName("SupportGroup");
                    listPicker.IsEnabled = false;
                    _init = true;

                }
            }




        }
    }
}
