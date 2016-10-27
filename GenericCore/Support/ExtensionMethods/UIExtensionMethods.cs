using GenericCore.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GenericCore.Support
{
    public static class UIExtensionMethods
    {
        public static void BindEnumValues<TEnum>(this ComboBox combo, int selectedIndex)
        {
            combo.Items.Clear();

            Dictionary<int, string> map = new Dictionary<int, string>();
            int i = 0;

            var names = Enum.GetNames(typeof(TEnum)).ToList();
            names.ForEach(x => { map.Add(i++, x); });

            ComboItem[] comboValues =
                map
                    .ToEmptyIfNull()
                    .Select(x => new ComboItem(x.Value, x.Key))
                    .ToEmptyArrayIfNull();

            combo.Items.AddRange(comboValues);
            combo.SelectedIndex = selectedIndex;
        }

        public static void ShowError(this Form form, string text, string title = null)
        {
            form.AssertNotNull("form");

            string caption = (title.IsNull()) ? "Error" : title;
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
        }

        public static void Show(this Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
        }
    }
}
