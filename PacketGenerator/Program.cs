using System.Xml;

namespace PacketGenerator;

class Program
{
    private static string genPackets;
    private static ushort packetId;
    private static string packetEnums;
        
    private static string serverRegister;
    private static string clientRegister;
    static void Main(string[] args)
    {
        string packetLauncherPath = "../../PacketLauncher.xml";
        
        XmlReaderSettings settings = new XmlReaderSettings()
        {
            IgnoreComments = true,
            IgnoreWhitespace = true
        };
            
        if(args.Length >= 1)
            packetLauncherPath = args[0];
            
        using (XmlReader r = XmlReader.Create(packetLauncherPath, settings))
        {
            r.MoveToContent();
    
            while (r.Read())
            {
                if (r.Depth == 1 && r.NodeType == XmlNodeType.Element)
                    ParsePacket(r);
            }

            string fileText = string.Format(PacketFormat.fileFormat, packetEnums, genPackets);
            File.WriteAllText("GenPackets.cs", fileText);
            string serverManagerText = string.Format(PacketFormat.managerFormat, serverRegister, "Server");
            File.WriteAllText("ServerPacketManager.cs", serverManagerText);
            string clientManagerText = string.Format(PacketFormat.managerFormat, clientRegister, "DummyClient");
            File.WriteAllText("ClientPacketManager.cs", clientManagerText);
        }
    }

    private static void ParsePacket(XmlReader r)
    {
        if (r.NodeType == XmlNodeType.EndElement)
            return;

        if (r.Name.ToLower() != "packet")
        {
            Console.WriteLine("Invalid packet node");
            return;
        }

        string packetName = r["name"];
        if (string.IsNullOrEmpty(packetName))
        {
            Console.WriteLine("Packet without name.");
            return;
        }

        Tuple<string, string, string> t = ParseMembers(r);
        genPackets += string.Format(PacketFormat.packetFormat,
            packetName, t.Item1, t.Item2, t.Item3);
        packetEnums += string.Format(PacketFormat.packetEnumFormat, packetName, ++packetId) + Environment.NewLine + "\t";
        
        if(packetName.StartsWith("S_") || packetName.StartsWith("s_"))
            clientRegister += string.Format(PacketFormat.managerRegisterFormat, packetName) + Environment.NewLine;
        else if(packetName.StartsWith("C_") || packetName.StartsWith("c_"))
            serverRegister += string.Format(PacketFormat.managerRegisterFormat, packetName) + Environment.NewLine;
        else
        {
            Console.WriteLine("Check packet name. Is it starts with \"S_\" or \"C_\"?");
            return;
        }
    }

    private static Tuple<string,string,string> ParseMembers(XmlReader r)
    {
        string packetName = r["name"];

        string memberCode = "";
        string readCode = "";
        string writeCode = "";
        
        int depth = r.Depth + 1;
        while (r.Read())
        {
            if (r.Depth != depth)
                break;
            
            string memberName = r["name"];
            if (string.IsNullOrEmpty(memberName))
            {
                Console.WriteLine("Member without name.");
                return null;
            }

            if(string.IsNullOrEmpty(memberCode) == false)
                memberCode += Environment.NewLine;
            if(string.IsNullOrEmpty(readCode) == false)
                readCode += Environment.NewLine;
            if(string.IsNullOrEmpty(writeCode) == false)
                writeCode += Environment.NewLine;
            
            string memberType = r.Name.ToLower();
            switch (memberType)
            {
                case "byte":
                case "sbyte":
                    memberCode += string.Format(PacketFormat.memberFormat, memberType, memberName);
                    break;
                case "bool":
                case "char":
                case "double":
                case "float":
                case "Half":
                case "int":
                case "Int128":
                case "long":
                case "short":
                case "uint":
                case "UInt128":
                case "Ulong":
                case "ushort":
                    memberCode += string.Format(PacketFormat.memberFormat, memberType, memberName);
                    readCode += string.Format(PacketFormat.readFormat, memberName);
                    writeCode += string.Format(PacketFormat.writeFormat, memberName);
                    break;
                default:
                    break;
            }
        }
        
        memberCode = memberCode.Replace("\n", "\n\t");
        readCode = readCode.Replace("\n", "\n\t\t");
        writeCode = writeCode.Replace("\n", "\n\t\t");
        return new Tuple<string, string, string>(memberCode, readCode, writeCode);
    }
}