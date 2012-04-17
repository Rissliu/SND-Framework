using System;
using System.Collections.Generic;
using System.Text;
using SND.KQ.Machines.DevInterface;

namespace SND.KQ.BL
{
    public class MachineFactory
    {

        public static IMachine GetMachine(string machineType, string ip, string port, string userName, string password, string machineNum)
        {
            if (machineType.ToUpper() == "MACHINE")
            {
                return new  Machine(ip, port, userName, password, machineNum);
            }
            else if (machineType.ToUpper() == "EACM")
            {
                return new EACMMachine(ip, port, userName, password, machineNum);
            }
            else
            {
                return new Machine(ip, port, userName, password, machineNum);
            }
        }
    }
}
