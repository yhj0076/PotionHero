START ../PacketGenerator/bin/net9.0/PacketGenerator.exe ../PacketGenerator/PacketLauncher.xml
XCOPY /Y GenPackets.cs "../Server/Packet"
XCOPY /Y GenPackets.cs "../DummyClient/Packet"
XCOPY /Y GenPackets.cs "C:\Users\yhj00\Connection\Assets\Script\_ServerControl\Client\Packet"

XCOPY /Y ServerPacketManager.cs "../Server/Packet"
XCOPY /Y ClientPacketManager.cs "../DummyClient/Packet"
XCOPY /Y ClientPacketManager.cs "C:\Users\yhj00\Connection\Assets\Script\_ServerControl\Client\Packet"