# ReadMe.md

![logo.png](logo.png)

Our C# HTTP Web3 Client.

> “The ~~cloud~~ *protocol* we use to keep your files.”
(C2 ― "Storage")
> 

---

## WHAT:

It communicates with the [IPFS](https://docs.ipfs.io/concepts/) network. You can write and read files from anywhere in the World.

- Content Addressable ([DAG](https://blog.infura.io/an-introduction-to-ipfs/))
- P2P networking ([PubSub](https://en.wikipedia.org/wiki/Publish%E2%80%93subscribe_pattern))
- Permanent/Persistent storage ([Pin](https://docs.ipfs.io/concepts/persistence))

<aside>
💡 C2IPFS is no less than an IPFS's API wrapper written in C#.

</aside>

---

## WHY:

IPFS's most used libraries are either [javascript](https://github.com/ipfs/js-ipfs) or [go](https://github.com/ipfs/go-ipfs). Unfortunately, these languages aren't compatible with .NET and other C# environment.

---

## WHO:

The repository's target is anyone interested in using an IPFS network with C# language. Whether you're building a .NET app or some game with Unity and require decentralized storage, you are in the right place.

<aside>
👥 The team working at these modules are network architects, full-stack developers with js experience. TDD appreciated.

</aside>

---

## WHEN:

CipherDuo’s IPFS development stage type is [Architecture](https://drive.google.com/file/d/1EPL4WBrfO4kD4iK85P471DgZCAx0XhpX/view?usp=sharing). It has a core team that extends & updates the system twice a year for ~2 months each time.

---

## WHERE:

Hop on [Discord](https://discord.com/invite/wBEbPMkrpW) & get in touch to start. When you're confident of your contribution, apply on our [recruiting page](https://job.cipherduo.org/) to join the core team. Else, you can check our [contribution guide](https://github.com/CipherDuo/C2IPFS/blob/master/CONTRIBUTING.md), fork this repository and document pull requests & issues as we go.

---

## HOW:

We start from [RichardSchneider](https://github.com/richardschneider)’s [net-ipfs-HTTP-client](https://github.com/richardschneider/net-ipfs-http-client). After a few updates, we hold the bare minimum functions. We write #regions for architectures not compatible with [System.Net](http://system.net/) (WebGL). Tests!

---

CipherDuo IPFS is released under [BSL 1.1](https://github.com/CipherDuo/C2IPFS/blob/master/CONTRIBUTING.md)