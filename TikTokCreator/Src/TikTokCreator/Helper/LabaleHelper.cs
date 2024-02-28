﻿namespace TikTokCreator.Helper
{
    public class LabaleHelper
    {
        public static void WriteLabale(Label label, string messager, int type = 0)
        {

            if (messager != null)
            {
                switch (type)
                {
                    case 0:
                        {
                            label.Invoke((MethodInvoker)delegate
                            {
                                label.ForeColor = Color.Black;
                                label.Text = messager;
                            });
                            break;
                        }
                    case 1:
                        {
                            label.Invoke((MethodInvoker)delegate
                            {
                                label.ForeColor = Color.Pink;
                                label.Text = messager;
                            });
                            break;
                        }
                    case 2:
                        {
                            label.Invoke((MethodInvoker)delegate
                            {
                                label.ForeColor = Color.Green;
                                label.Text = messager;
                            });
                            break;
                        }
                }
            }
          
        }
    }
}
