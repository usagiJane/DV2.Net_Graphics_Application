using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DV2.Net_Graphics_Application
{
    public partial class MainForm
    {
        /// <summary>
        /// ペンタブレット移動動作イベント関数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TabletMouseMove(object sender, MouseEventArgs e)
        {
            int mouseX, mouseY;
            //for Debug
            //LogOutput("Mouse Position is  ---> " + Cursor.Position.X.ToString() + "," + Cursor.Position.Y.ToString() + " <---");
            mouseX = Cursor.Position.X;
            mouseY = Cursor.Position.Y;

            mouseX = (mouseX - 73) / 5;
            mouseY = (mouseY - 180) / 4;

            if (mouseX < 0)
            {
                mouseX = 0;
            }

            if (mouseY < 0)
            {
                mouseY = 0;
            }

            if (mouseX + 0 >= picBox.Width)
            {
                mouseX = picBox.Width - 0;
            }

            if (mouseY + 0 >= picBox.Height)
            {
                mouseY = picBox.Height - 0;
            }


            //for Debug
            //codeOutput("Mouse Position is  ---> " + Cursor.Position.X.ToString() + "," + Cursor.Position.Y.ToString() + " <---" + "The Fixed Mouse Position is  ---> " + mouseX + "," + mouseY + " <---");

            movement.X = mouseX;
            movement.Y = mouseY;
            DotDataInitialization(ref forDisDots);

            for (int width = 0; width < 48; width++)
            {
                for (int height = 0; height < 32; height++)
                {
                    forDisDots[width, height] = allDotData[movement.X + width, movement.Y + height];
                }
            }
            Dv2Instance.SetDots(forDisDots, BlinkInterval);
            label_posX.Text = movement.X.ToString();
            label_posY.Text = movement.Y.ToString();
        }
    }
}
