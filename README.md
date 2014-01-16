BitcoinLib
==========

**C# Bitcoin Qt RPC Wrapper for .Net 4.5+ projects**


Features
--------

- Fully compatible with QT's 0.8.6 RPC API.
- Console and web test clients with demo methods implemented.
- Extended methods for cases that the RPC API falls short.
- Testnet ready.

Instructions
------------

- Locate your `bitcoin.conf` file (in Windows it's under: `%AppData%\Roaming\Bitcoin`) and add these lines:
	- rpcuser = myRpcUsername
	- rpcpassword = myRpcPassword
	- daemon=1
	- txindex=1

- Call `bitcoind -reindex -txindex -debugnet -printtoconsole` and wait until it's finished reindexing (it might take a while). You need to do this just once.

- Shut down bitcoind and start it again with these arguments: `bitcoind -daemon -debugnet -printtoconsole`. Append `-testnet -port 18332` if you want to run it for Testnet. Wait until it is fully synchronized. 

- Edit the .config files in the solution to fit your needs
	- `app.config` for the Console test client
	- `web.config` for the Web test client

  Make sure you update the `bitcoin.conf` as well when you alter `RpcUser` and `RpcPassword` 

- You're good to go.

Based on
--------

- Bitnet, by Konstantin Ineshin (http://bitnet.sourceforge.net)
- BitcoinRpcSharp, by BitKoot (https://github.com/BitKoot/BitcoinRpcSharp)
- Bitcoin-wrapper, by Lars Holdgaard (https://github.com/LarsHoldgaard/bitcoin-wrapper)