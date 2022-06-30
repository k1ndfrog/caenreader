using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.caen.RFIDLibrary;
using System.Threading;
namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
       
                //connecting to reader 
                CAENRFIDReader MyReader = new CAENRFIDReader();
                MyReader.Connect(CAENRFIDPort.CAENRFID_RS232, "COM3");
                CAENRFIDLogicalSource MySource = MyReader.GetSource("Source_0");

                    //setting power
                    double Gain = 5.0;
                    double Loss = 0.1;
                    double ERPower = 376.0;
                    int Outower;
                    Outower = (int)(ERPower / Math.Pow(10, ((Gain - Loss - 2.14) / 10.0)));
                    MyReader.SetPower(Outower);
                    Console.WriteLine("set erp= " + ERPower);
                    Console.WriteLine("set out= " + (double)Outower);
            
                //double checking power out is what we set it to
                double ERPPower;
                int OutPower;
                OutPower = MyReader.GetPower();
                ERPPower = ((double)OutPower) * ((double)Math.Pow(10, ((Gain - Loss - 2.14) / 10)));
                Console.WriteLine("reading erp= "+ERPPower);
                Console.WriteLine("reading out= "+OutPower);

            {

                CAENRFIDTag[] MyTags = MySource.InventoryTag();

                for (int i = 0; i < MyTags.Length; i++)
                {
                    //outputting EPC 
                    String s = BitConverter.ToString(MyTags[i].GetId());
                    Console.WriteLine(s);
                    String EPCString = BitConverter.ToString(MyTags[i].GetId());

                    //writing
                    Console.WriteLine(EPCString);
                    UTF8Encoding Enc = new UTF8Encoding();
                    //byte[] DataToWrite = Enc.GetBytes("0x8005");
                    //MySource.WriteTagData_EPC_C1G2(MyTags[i], 3, 241, 6, DataToWrite);
                    //Console.WriteLine("Tag written!");

                    //reading 
                    byte[] DataToRead;
                    DataToRead = MySource.ReadTagData_EPC_C1G2(MyTags[i], 3, 244, 4);
                    char[] characters = System.Text.Encoding.ASCII.GetChars(DataToRead);
                    Console.WriteLine("byte array: " + BitConverter.ToString(DataToRead));
                    Console.WriteLine("Temp Sensor Control Word 1 reading = " + new string(characters));
                    Console.WriteLine(DataToRead);

                    //determining address
                    String n = BitConverter.ToString(MyTags[i].GetId());

                    //byte[] DataToRea;
                    //DataToRea = MySource.ReadTagData_EPC_C1G2(MyTags[i], 3, 236, 6);
                    //char[] chars = System.Text.Encoding.ASCII.GetChars(DataToRea);
                    //Console.WriteLine("byte array: " + BitConverter.ToString(DataToRea));
                    //Console.WriteLine("Battery Management Word 2 = " + new string(chars));
                }

            }

            //looping through to check connection distance 
            for (int j = 0; j < 99999999; j++)
            {
                CAENRFIDTag[] MyTags = MySource.InventoryTag();

                if (MyTags != null)

                {
                    Console.WriteLine("I am reading the tag");
                }

                else
                {
                    Console.WriteLine("undetected");
                }

                Thread.Sleep(1000);
            }

                // disconnectong from reader 
                Console.ReadKey();
                MyReader.Disconnect();
            }

        
      
    }
}
