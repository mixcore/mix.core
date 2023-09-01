# Azure Event Grid Notes:

### Ref: https://learn.microsoft.com/en-us/azure/event-grid/mqtt-publish-and-subscribe-portal

- Make sure that port 8883 is open in your firewall.
- You need an X.509 client certificate to generate the thumbprint and authenticate the client connection.

### Copilot suggestion:

```
Both MQTT and SignalR are messaging protocols that serve different purposes and excel in different scenarios. MQTT is a lightweight, publish-subscribe messaging protocol designed for low-bandwidth, high-latency networks, while SignalR is a library for ASP.NET developers that makes developing real-time web functionality easy.

MQTT is best suited for low-bandwidth, high-latency networks, often found in IoT and industrial automation scenarios. It works over TCP/IP and supports various Quality of Service (QoS) levels, ranging from at-most-once to exactly-once message delivery 2. On the other hand, SignalR is best suited for web-based real-time applications, where low latency and high responsiveness are crucial. Common use cases include web-based chat applications.

If you are looking to develop a real-time application that requires low latency and high responsiveness, SignalR might be a better choice. However, if you are working with IoT devices or industrial automation systems that require efficient communication between machines and control systems over low-bandwidth networks, MQTT might be a better choice.

```
