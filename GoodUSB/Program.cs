using System;
using System.ComponentModel;
using System.Management;
using System.Threading;
using System.Windows.Forms;
using System.Drawing;

namespace GoodUSB
{
    internal class VerifyForm : Form
    {
        public Label Label;
        public VerifyForm()
        {
            Width = 1000;
            Height = 500;
            Label = new Label
            {
                Text = "A new keyboard has been inserted. Press Control+Alt+Delete to unlock input.",
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill,
                Font = new Font("Arial", 20)
            };
            Controls.Add(Label);
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            Close();
        }
    }

    internal class Program
    {
        public partial class NativeMethods {

            /// Return Type: BOOL->int
            ///fBlockIt: BOOL->int
            [System.Runtime.InteropServices.DllImportAttribute("user32.dll", EntryPoint="BlockInput")]
            [return: System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.Bool)]
            public static extern  bool BlockInput([System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.Bool)] bool fBlockIt) ;

        }

        private static void DeviceInsertedEvent(object sender, EventArrivedEventArgs e)
        {
            var instance = (ManagementBaseObject)e.NewEvent["TargetInstance"];
            var instance_id = instance.GetPropertyValue("DeviceID");
            Console.WriteLine(instance_id);
            var searcher = new ManagementObjectSearcher("Select * from Win32_Keyboard");

            foreach(ManagementObject keyboard in searcher.Get())
            {
                var keyboard_id = keyboard.GetPropertyValue("DeviceID").ToString().Split('\\')[1];
                Console.WriteLine(keyboard_id);
                if (keyboard.GetPropertyValue("DeviceID").ToString().Split('\\')[1].StartsWith(instance_id.ToString().Split('\\')[1])) //PID & VID are identical
                {
                    //A keyboard was inserted
                    Console.WriteLine("Keyboard inserted...");
                    NativeMethods.BlockInput(true);
                    Application.EnableVisualStyles();
                    Application.Run(new VerifyForm());
                    return;
                }
            }
        }


        static void Main(string[] args)
        {           
            var insertQuery = new WqlEventQuery("SELECT * FROM __InstanceCreationEvent WITHIN 2 WHERE TargetInstance ISA 'Win32_USBHub'");
            var insertWatcher = new ManagementEventWatcher(insertQuery);
            insertWatcher.EventArrived += new EventArrivedEventHandler(DeviceInsertedEvent);
            insertWatcher.Start();
            while (true){}
        }
    }
}