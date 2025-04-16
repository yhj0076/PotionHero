START ../PacketGenerator/bin/net9.0/PacketGenerator.exe ../PacketGenerator/PacketLauncher.xml
XCOPY /Y GenPackets.cs "../Server/Packet"
XCOPY /Y GenPackets.cs "../DummyClient/Packet"

XCOPY /Y ServerPacketManager.cs "../Server/Packet"
XCOPY /Y ClientPacketManager.cs "../DummyClient/Packet"