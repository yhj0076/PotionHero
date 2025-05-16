using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using ServerCore.Utility;

public class NetworkInterfaceHelper
{
    public IPAddress GetWifiLinkLocalIPv6()
    {
        // 현재 기기에 연결된 모든 네트워크 인터페이스 목록을 가져온다.
        // 그 중에서 조건에 맞는 첫 번째 인터페이스를 선택하거나 없으면 null 반환한다.
        // 조건1 : 무선 LAN 인터페이스 타입
        // 조건2 : 현재 활성화(연결)된 인터페이스만 선택
        // wifi가 연결되어 있지 않거나 해당 인터페이스가 없으면 null 반환
        
        // 링크-로컬 IP 주소 반환
        
        var wifiInterface = NetworkInterface.GetAllNetworkInterfaces() 
            .FirstOrDefault(ni =>
                ni.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 &&
                ni.OperationalStatus == OperationalStatus.Up);
        
        if(wifiInterface == null)
            return null;
        
        var linkLocalIPv6 = wifiInterface.GetIPProperties()
            .UnicastAddresses
            .Where(addr =>
                addr.Address.AddressFamily == AddressFamily.InterNetworkV6 &&
                addr.Address.IsIPv6LinkLocal)
            .Select(addr => addr.Address.ToString())
            .FirstOrDefault();

        var address = IPAddress.Parse(linkLocalIPv6);
        
        var ipv6Props = wifiInterface.GetIPProperties();
        if(ipv6Props != null)
            address.ScopeId = ipv6Props.GetIPv6Properties().Index;
        return address;
    }

    public IPAddress GetEthernetIPv4()
    {
        var ethernetInterface = NetworkInterface.GetAllNetworkInterfaces()
            .FirstOrDefault(ni =>
                ni.NetworkInterfaceType == NetworkInterfaceType.Ethernet &&
                ni.OperationalStatus == OperationalStatus.Up);
        
        if (ethernetInterface == null)
            return null;
        
        var ipv4Address = ethernetInterface.GetIPProperties()
            .UnicastAddresses
            .Where(addr =>
                addr.Address.AddressFamily == AddressFamily.InterNetwork &&
                !IPAddress.IsLoopback(addr.Address) &&
                !addr.Address.ToString().StartsWith("169.254."))
            .Select(addr => addr.Address)
            .FirstOrDefault();
        
        return ipv4Address;
    }
}
