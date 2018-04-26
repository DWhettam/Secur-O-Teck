# Secur-O-Teck

A client/server system for managing users and performing basic cryptography. 

.NET Web API is used to communicate between the client and server. A Entity Framework code-first database is used to manage and store user information.

#### Cryptography
* SHA1 + SHA256 hashing
* SHA1 message signing
* Symmetric and asymmetric cryptography (RSA and AES) to securely send data between the client and server


#### Usage
The client can input the following commands:

* TalkBack Hello
  * Get a "Hello World" message from the server
* TalkBack Sort [<int>, <int>, <int>, <int>]
  * The servers sorts the recieved integers and returns the sorted list to the client
* User Post <name>
  * Sends a POST request to the server, adding a user to the database
* User Get <name>
  * Sends a GET request to the server, confirming if the user is present in the database
* User Set <name> <apikey>
  * Stores the specified username and API Key in the client
* User Delete
  * Deletes the user currently stored in the client from the database
* Protected Hello
  * Confirms if a user exists in the database with the given API Key
* Protected SHA1 <message>
  * Returns the SHA1 hash of the input message
* Protected SHA256 <message>
  * Returns the SHA256 hash of the input message
* Protected Get PublicKey
  * Stores the public key on the client-side
* Protected Sign <message>
  * Signs the input message
* Protected AddFifty <integer>
  * Client sends an RSA encrypted value using the servers public RSA key
  * Server decryptes the message with its private RSA key, adds fifty, encrypts the message with the client's AES key and sends back to the client
  * Client decrypts the message using it's AES key



### Requirements
* .NET Web API
* Entity Framework

Part of the Univerity of Hull Distributed Systems Programming module
