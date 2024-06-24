This repository illustrates the use of PACT framework for testing in .NET projects

# Pre-requisites
  - Any .NET supported IDE (Visual Studio, VS Code, JetBrains Rider etc)
  - .NET SDK (8 and above). However, project can be run with .NET 6 as well with small tweaks
  - RabbitMQ should be installed on localhost default port. This is optional in case one does not want to actually run the projects.

# How to run
  - Clone the repo
  - Go to solution root directory
  - Run `dotnet build`
  - Once the build is successful, run `dotnet test`

One may visit the blog post for more understanding of pact and its implementation in this repository https://www.linkedin.com/pulse/contract-testing-apis-intro-pact-net-core-ajay-kumar-nk3jf/?trackingId=rrCodMNgREK%2BKK82pRBsQw%3D%3D
