using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace WP8_Sphero
{
    public class ColorLedCommand
    {
        Color theColor;
        
        public ColorLedCommand(Color color)
        {
            theColor = color;
        }

        // sample package is 255, 255, 2, 32, 1, 4, 0, 0, 0, 216
        // sample package is   A,   R, G,  B, 1, 4, 0, 0, 0, 216

        public byte[] ToPacket()
        {
            int checksum = 255 - ((2 + 32 + 1 + 5 + theColor.R + theColor.G + theColor.B + 0) % 256);

            List<byte> byteList = new List<byte>();

            byteList.Add((byte)(255)); // SOP1
            byteList.Add((byte)(255)); // SOP2
            byteList.Add((byte)(2)); // DID
            byteList.Add((byte)(32)); // CID
            byteList.Add((byte)(1)); // SEQ
            byteList.Add((byte)(5)); // DLEN
            
            byteList.Add((byte)(theColor.R)); // RED
            byteList.Add((byte)(theColor.G)); // GREEN
            byteList.Add((byte)(theColor.B)); // BLUE
            
            byteList.Add((byte)(0)); // FLAG
            
            byteList.Add((byte)(checksum)); // CHK

            byte[] bytes = byteList.ToArray();

            return bytes;
        }

    }

    class BackLedCommand
    {
        int theBright;

        public BackLedCommand(int bright)
        {
            theBright = bright;
        }

        public byte[] ToPacket()
        {
            int checksum = 255 - ((2 + 33 + 1 + 2 + theBright) % 256);

            List<byte> byteList = new List<byte>();

            byteList.Add((byte)(255)); // SOP1
            byteList.Add((byte)(255)); // SOP2
            byteList.Add((byte)(2)); // DID
            byteList.Add((byte)(33)); // CID
            byteList.Add((byte)(1)); // SEQ
            byteList.Add((byte)(2)); // DLEN

            byteList.Add((byte)(theBright)); // BRIGHT

            byteList.Add((byte)(checksum)); // CHK

            byte[] bytes = byteList.ToArray();

            return bytes;
        }

    }

    class SetHeading
    {
        int theHeading;

        public SetHeading(int heading)
        {
            theHeading = heading;
        }

        public byte[] ToPacket()
        {
            byte[] headingBytes = BitConverter.GetBytes(Convert.ToInt16(theHeading));
            byte lowByte = (byte)(theHeading & 0xff);
            byte highByte = (byte)((theHeading >> 8) & 0xff);

            int checksum = 255 - ((2 + 1 + 1 + 3 + highByte + lowByte) % 256);

            List<byte> byteList = new List<byte>();

            byteList.Add((byte)(255)); // SOP1
            byteList.Add((byte)(255)); // SOP2
            byteList.Add((byte)(2)); // DID
            byteList.Add((byte)(1)); // CID
            byteList.Add((byte)(1)); // SEQ
            byteList.Add((byte)(3)); // DLEN

            byteList.Add((byte)(highByte)); // HEADING(16bit)1
            byteList.Add((byte)(lowByte)); // HEADING(16bit)2

            byteList.Add((byte)(checksum)); // CHK

            byte[] bytes = byteList.ToArray();

            return bytes;
        }

    }

    class Roll
    {
        int theHeading;
        int theSpeed;

        public Roll(int heading, int speed)
        {
            theHeading = heading;
            theSpeed = speed;
        }

        public byte[] ToPacket()
        {
            //byte[] headingBytes = BitConverter.GetBytes(Convert.ToInt16(theHeading));
            byte lowByte = (byte)(theHeading & 0xff);
            byte highByte = (byte)((theHeading >> 8) & 0xff);

            int checksum = 255 - ((2 + 48 + 1 + 5 + theSpeed + highByte + lowByte + 1) % 256);

            List<byte> byteList = new List<byte>();

            byteList.Add((byte)(255)); // SOP1
            byteList.Add((byte)(255)); // SOP2
            byteList.Add((byte)(2)); // DID
            byteList.Add((byte)(48)); // CID
            byteList.Add((byte)(1)); // SEQ
            byteList.Add((byte)(5)); // DLEN

            byteList.Add((byte)(theSpeed)); // SPEED
            byteList.Add((byte)(highByte)); // HEADING(16bit)1
            byteList.Add((byte)(lowByte)); // HEADING(16bit)2

            byteList.Add((byte)(1)); // STATE

            byteList.Add((byte)(checksum)); // CHK

            byte[] bytes = byteList.ToArray();

            return bytes;
        }

    }

}
