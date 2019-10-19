# DMEE-View1
Draftsman-EE Schematic Viewer

## Abstract
This C# project is being written as a way to view and print Draftsman-EE "DMEE" Schematic (.SCH) and other DMEE drawing files on
a modern PC running Microsft Windows (r). The Draftsman-EE software may also be known as DMEE, DC-CAD, DCCAD, or DC/CAD IV. As of this writing, the viewer is a work in progress.

## Status
The present version is an early work-in-progress that does not yet support schematics with multiple pages, printing, or inserting modules (parts) from libraries or other files. The Oct. 19, 2019 update added support for drawing arcs and locating pins along with a simple set of zoom levels under the File menu. 

Text is presently drawn using Microsoft fonts instead of using Draftsman-EE's vector drawn character library. It does not yet include support for scaling and centering the file to fit the viewing area. These capabilities are all under development.

Program development is first focusing on drawing schematic and part files. Consequently, it does not handle drawing routes or handle layers. Since the program also doesn't insert modules from libraries (such as pad definitions) if you try to draw any PCB files you're likely to find them pretty empty except for a view of the lines and text from all layers simultaneously. 

## Use
Select an .SCH or other drawing file using the file menu and press the "Parse" button. The hide/show info button provides some additional information about the file. A few example files are provided in Sample Drawing Files directory to demonstrate the present level of development.

##  Background
I started developing this project to view some schematics that I created in the early 1990's using
Draftsman-EE version 4.09 running on MS-DOS with the goal of also being able to print and archive them as .pdf files.

Draftsman-EE was a PC-based schematic editor and PCB layout software package by
Design Computation, Inc. It was offered from the late 1980's to early 1990's.
Both the software and thecompany went away before the early days of the internet and the software was not widely advertised or reviewed. Consequently, internet searches turn up
almost no references to the company or the Draftsman-EE software, let alone any publicly available
archives of the software. 

### What was offered back in the day
According to _Choosing A PCB Layout System_, MICRO CORNUCOPIA, #45, Jan-Feb 1989, the software was available in multiple levels using different names and prices (10/1988 pricing shown) based mainly on the inclusion of an autorouter and the autorouter's capabilities. The base package did not include any autorouting capability.
- Draftsman E.E. $695.00
- DC Design $1195.00
- DC 810 $1995.00
- DC CAD $3495.00

##  Overview of the software
The Draftsman-EE schematic files, and much of the associated library files, consist of
lines of printable ASCII text. The text present a series of "command lines" that indicate how lines and text should be drawn, including
parameters such as coordinates, color, and scaling factors, along with whether additional 'modules' should be included from another file
or library. The viewer program parses these commands and their parameters in order to draw the content to the screen. 

## Development Environment and Executables
The project is presently being developed and tested as a Windows "Forms" application using C#
and .NET Framework 4.6.1 using Microsoft Visual Studio Community Edition, 2017 on Windows 10 64-bit.
