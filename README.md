# DrillSergeant

Get a tally of contributors' commits.

Written by Wyatt J. Miller, copyright 2022

Licensed by the Mozilla Public License v2.0 (see [LICENSE](./LICENSE.md))

###### Table of Contents

- [Overview](#overview)
- [Usage](#usage)
- [Installation](#installation)
  - [Release](#release)
  - [Source](#source)
    - [Prerequisites](#prerequisites)
    - [Prepare](#prepare)
    - [Build](#build)
- [Known Issues](#known-issues)
- [Next Steps/Contributing](#next-steps)
- [Troubleshooting](#troubleshooting)
- [Attribution](#attribution)

## Overview

When I was in university, I wanted a program that would tally up my git commits in a certain project that I'm working on. Based on search engine ninja skills, I couldn't find a program that did this. I decided to scratch my own itch.

I present DrillSergeant. This is a command-line application to tally up commits, ordered by commit count per contributor. In other words, DrillSergeant orders the contributors by how many commits they have made in descending order. For example, the first contributor listed will have the most commits in the project. The second contributor will have the second most commits, and so on.

## Usage

To use this command-line application, you must be in a git project. Otherwise, a nasty error will appear and you won't get your report. You can filter by branch or by tag (cannot be both, another nasty error appears here). You can get your report in your terminal, in a spreadsheet, or a PDF.

### Command-line arguments

```bash
  -o, --output <pdf|stdout|xlsx>  Specify the output given to the user
  -b, --branch <branch>           Specify the branch to filter by
  -t, --tag <tag>                 Specify the tag to filter by
```

Throw the `-h` flag for quick assistance. Throw the `--version` flag for version information.

### Examples

To get a commit report to your terminal:

```bash
drillsergeant -o stdout
```

Or you can simply:

```bash
drillsergeant
```

To get a commit report to your terminal filtered by the `devel` branch:

```
drillsergeant -b devel
```

To get a commit report to a spreadsheet:

```bash
drillsergeant -o xlsx
```

To get a commit report to a PDF file filtered by the `devel` branch:

```bash
drillsergeant -o pdf -b devel
```

Got too many contributors to fit onto your terminal? Run this by installing [bat](https://github.com/sharkdp/bat):

```bash
drillsergeant | bat
```

## Installation

There's are two forms of installation: releases and building from sources. This readme goes through both.

### Release

You may get releases from the [releases page](https://scm.wyattjmiller.com/wymiller/DrillSergeant/releases). This is the recommended way to start using DrillSergeant.

There will be three separate downloads: Windows (x86 64-bit), Linux (x86 64-bit), and Linux (ARM 64-bit), dubbed win64, linux64, and linuxaarch64 respectively.

Once downloaded and extracted, you can move it to your `$PATH`.

On Windows (Powershell):

```powershell
Copy-Item drillsergeant C:\Windows\
```

On Linux:

```bash
cp drillsergeant /usr/local/bin
```

The following commands assume you have elevated privileges.

If you do not have any of these platforms, read on to source installation as that's the next best option.

### Source

#### Prerequisites

You need the following to be able to build DrillSergeant:

- .NET 6 (available for Windows, macOS, and modern Linux distributions)
- git

#### Prepare

First, clone the repository with git:

```bash
git clone https://scm.wyattjmiller.com/wymiller/DrillSergeant
```

Then, change directories to where you've cloned DrillSergeant:

```bash
cd path/to/DrillSergeant
```

#### Build

Then, to build a full release of DrillSergeant (you can change the runtime to whatever fits your platform. You can learn more about this [here](https://docs.microsoft.com/en-us/dotnet/core/rid-catalog)):

```bash
dotnet publish DrillSergeant.csproj --configuration Release --framework net6.0 --output publish --self-contained True --runtime linux-x64 --verbosity Normal /property:PublishTrimmed=True /property:PublishSingleFile=True /property:IncludeNativeLibrariesForSelfExtract=True /property:DebugType=None /property:DebugSymbols=False
```

Then, you can find the binary in the `publish/` directory. You can move this executable to somewhere in the `$PATH` or you make a new enviroment variable to be integrated into your `$PATH`.

## Known Issues

- A user must be in the root of a git project in order for this program to run.

## Next Steps/Contributing

- See the [contributing](./CONTRIBUTING.md) file!

## Troubleshooting

Please file a issue on the issue page and I will get back with you as soon as possible.

## Attribution

Thank you to the developers, engineers, project managers, and contributors of the following projects - you make this program possible!

- [LibGit2Sharp](https://github.com/libgit2/libgit2sharp/)
- [OpenXML](https://docs.microsoft.com/en-us/office/open-xml/open-xml-sdk)
- [System.CommandLine](https://github.com/dotnet/command-line-api)
- [Pastel](https://github.com/silkfire/Pastel)
- [PdfSharpCore](https://github.com/ststeiger/PdfSharpCore)

