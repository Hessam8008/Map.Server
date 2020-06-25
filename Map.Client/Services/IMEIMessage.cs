namespace Map.Client.Services
{
    using System;

    using Map.Client.Interfaces;
    using Map.Client.Models;

    public class IMEIMessage : IMessage<IMEI>
    {
        public IMEIMessage()
        {
            //Console.WriteLine($"{nameof(IMEIMessage)} Created.");
        }

        public IMEI MessageObject { get; private set; }

        public virtual bool CanParse(PlainMessage msg)
        {
            if (msg.Type != "IMEI") return false;
            this.MessageObject = new IMEI { Value = msg.OBJ };
            return true;
        }

        public virtual void PrintResult()
        {
            var temp_backColor = Console.BackgroundColor;
            var temp_foreColor = Console.ForegroundColor;

            Console.BackgroundColor = ConsoleColor.Green;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"♫ IMEI: {this.MessageObject.Value}");
            
            Console.BackgroundColor = temp_backColor;
            Console.ForegroundColor = temp_foreColor;
        }
    }
}