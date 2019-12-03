# DMEE-View1
Draftsman-EE Schematic Viewer

## Abstract
This C# project is being written as a way to view and print Draftsman-EE "DMEE" Schematic (.SCH) and other DMEE drawing files on
a modern PC running Microsft Windows (r). The Draftsman-EE software may also be known as or associated with the names DMEE, DC-CAD, DCCAD, DC Design, DC 810, or DC/CAD IV. As of this writing, the viewer is a work in progress, Refer to following for details.

## Status
12/3/2019: The program enables viewing and printing DMEE schematic drawings. The latest version has added a print setup dialog that provides options for selecting the printer, setting margins, scaling (Zoom), and aligning the drawing boundaries to the print page prior to printing.

## Limitations
The present version is still a work-in-progress that does not yet support schematics with multiple pages, solely because I only have single page schematic samples to test with.

Text is presently drawn using Microsoft fonts instead of using Draftsman-EE's vector drawn character library. The progam DOES NOT support drawing PCB routes and patterns at this time, nor does it support multiple layers. Thus, .PCB files will not display much, if anything.

## Use
The search folders for drawing and library (.LBR) files are configured under Configuration menu. Select a .SCH or other drawing file using the file menu. The working and library folders will be searched for component modules called for by the drawing and the file will be automatically displayed. If a previous file had already been selected you can also press the Draw button to display it. The hide/show info button provides some additional information about the file. A number of different Zoom settings are available under the File menu along with the option to fit the drawing to the window.  A few example files are provided in Sample Drawing Files directory to demonstrate the present level of functionality.

##  Background
I started developing this project to view, print, and archive as .pdf files some schematics that I created in the early 1990's using
Draftsman-EE version 4.09 running on MS-DOS. After looking at the schematic and library data files it looked like it would be feasible, interesting, and potentially less frustrating to reverse engineer the format enough to create a Windows 10 program for viewing and printing them rather than trying to get the software and its outdated video and printer drivers running under something like DOSBox or VirtualBox.

Draftsman-EE was a PC-based schematic editor and PCB layout software package by
Design Computation, Inc. It was offered from the late 1980's to early 1990's.
Both the software and thecompany went away before the early days of the internet and the software was not widely advertised or reviewed. Consequently, internet searches turn up
almost no references to the company or the Draftsman-EE software, let alone any publicly available
archives of the software. 

### What was offered back in the day
According to _Choosing A PCB Layout System_, MICRO CORNUCOPIA, #45, Jan-Feb 1989, the software was available in multiple levels using different names and prices (10/1988 pricing shown) based mainly on the inclusion of an autorouter and the autorouter's capabilities. The base package did not include any autorouting capability.
- Draftsman E.E. $695.00  - manual route up to 8" x 10" ?
- DC Design $1195.00 - manual route boards up to 32" x 32"
- DC 810 $1995.00 - autoroute boards up to 8" x 10"
- DC CAD $3495.00 - autoroute with rip-up, boards up to 32" x 32"

##  Overview of the software's operation
The Draftsman-EE schematic files, and much of the associated library files, consist of
lines of printable ASCII text. The text present a series of "command lines" that indicate how lines and text should be drawn, including
parameters such as coordinates, color, and scaling factors, along with whether additional 'modules' should be included from another file
or library. The viewer program parses these commands and their parameters in order to draw the content to the screen. 

## Development Environment and Executables
The project is presently being developed and tested as a Windows "Forms" application using C#
and .NET Framework 4.6.1 and built using Microsoft Visual Studio Community Edition, 2017/2019 on Windows 10 64-bit.
