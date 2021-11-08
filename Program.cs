using System;
using RohdeSchwarz.RsInstrument;
using System.Threading;
namespace IQmodulation
{
    class Program
    {
        static void Main(string[] args)
        {
            RsInstrument instr;
            try // Separate try-catch for initialization prevents accessing uninitialized object
            {
                //-----------------------------------------------------------
                // Initialization:
                //-----------------------------------------------------------
                //Adjust the VISA Resource string to fit your instrument
                  var resourceString1 = "TCPIP::169.254.108.161::INSTR"; // Standard LAN connection (also called VXI-11)
                // var resourceString1 = "TCPIP::169.254.108.161::hislip0"; // Hi-Speed LAN connection - see 1MA208
               // var resourceString1 = "GPIB::20::INSTR"; // GPIB Connection
                //var resourceString1 = "USB::0x0AAD::0x0088::101699::INSTR"; // USB-TMC (Test and Measurement Class)
                instr = new RsInstrument(resourceString1);
            }
            catch (RsInstrumentException e)
            {
                Console.WriteLine($"Error initializing the instrument session:\n{e.Message}");

                Console.Write("\nPress any key ...");
                Console.ReadKey();
                return;
            }
            string idn = instr.QueryString("*IDN?");
            Console.WriteLine($"Hello, I am: '{idn}'");
            Console.Write(idn);
            Console.Write("\n");

            // ******************************************************************
            // Reset instrument first
            // ******************************************************************
            instr.WriteString("*RST;*CLS");
            // ******************************************************************
            // Lock the instrument to the controller
            // ******************************************************************
            instr.QueryInteger("LOCK? 72349234");
            // Lock instrument to avoid interference by other controllers
            // Use an arbitrary number
            // Response: 1
            // Request granted, i.e. the instrument is locked
            // Abort program if request is refused
            // ******************************************************************
            // Select normal operation mode
            // ******************************************************************
            instr.WriteString("SOURce:OPMode NORMal ");
            // ******************************************************************
            // Set RF frequency and level
            // ******************************************************************
            instr.WriteString("SOURce:FREQuency:CW 80 MHz; SOURce:POWer -10dBm");
           // Console.Write("SOURce:FREQuency:CW 1.4 GHz");
            // :SOURce:PHASe 0
            // :SOURce:PHASe:REFerence
            //instr.WriteString("SOURce:POWer -10dBm");
   
            instr.QueryInteger("SOURce:POWer:PEP?");
            // ******************************************************************
            // Enable internal reference frequency source
            // ******************************************************************
            instr.WriteString("SOURce:ROSCillator:SOURce INTernal");
            // ******************************************************************
            // Enable internal LO source
            // ******************************************************************
            instr.WriteString("SOURce:LOSCillator:SOURce INT");
            // ******************************************************************
            // Define and enable impairments
            // Enable modulation
            // ******************************************************************

              instr.WriteString("SOURce:IQ:IMPairment:LEAKage:I -1; SOURce:IQ:IMPairment:LEAKage:Q 1; SOURce:IQ:IMPairment:IQRatio:MAGNitude 1");
               //instr.WriteString("SOURce:IQ:IMPairment:LEAKage:Q 1");
              // instr.WriteString("SOURce:IQ:IMPairment:IQRatio:MAGNitude 1");
               // Sets the gain imbalance to 1 %
               instr.QueryInteger("SOURce:IQ:IMPairment:IQRatio:MAGNitude?");
               // Response: 0.087 dB
               instr.WriteString("SOURce:IQ:IMPairment:QUADrature:ANGLe 2; SOURce:IQ:WBSTate ON");
               //instr.WriteString("SOURce:IQ:WBSTate ON");
               instr.WriteString("SOURce:IQ:CREStfactor 0.05");
               instr.WriteString("SOURce:IQ:IMPairment:STATe ON");
               instr.WriteString("SOURce:IQ:STATe ON");
            // ******************************************************************
            // Enable output of the generated signal at the RF connector
            // ******************************************************************
            instr.WriteString("OUTPut:STATe ON");
               // ******************************************************************
               // Unlock the instrument
               // ******************************************************************
               instr.WriteString("UNL 72349234");
               instr.QueryInteger("*OPC?");
              double i = 80;

            while (true)
            {
               
                    Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
                    string s1 = " SOURce:FREQuency:CW ";
                    string s2 = ((decimal)i).ToString();
                    string s3 = " MHz";
                    string s4 = string.Concat(s1, s2, s3);
                    Console.Write(s4 + '\n');
                    instr.WriteString(s4);
                    i += 0.1;
                    if (i >= 99)
                    {
                       i = 80;
                    }
            }

               

              /* instr.WriteString("SOURce:FREQuency:CW 1.5 GHz");
                instr.WriteString("SOURce:FREQuency:CW 2 GHz");
                instr.WriteString("SOURce:FREQuency:CW 2.5 GHz");
                instr.WriteString("SOURce:FREQuency:CW 3 GHz");
                instr.WriteString("SOURce:FREQuency:CW 3.5 GHz");
                instr.WriteString("SOURce:FREQuency:CW 4 GHz");
                instr.WriteString("SOURce:FREQuency:CW 4.5 GHz");
                instr.WriteString("SOURce:FREQuency:CW 5 GHz");*/
                
            Console.Write("\nPress any key ...");
            Console.ReadKey();

            instr.Dispose();

        }
    }
}
