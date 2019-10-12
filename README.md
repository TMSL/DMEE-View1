# DMEE-View1
Draftsman-EE Schematic Viewer

## Abstract
This C# project is being written as a way to view and print Draftsman-EE Schematic (.SCH) files on
a modern PC running Microsft Windows (r). As of this writing, it is a work in progress without a particular release version.

##  Background
I started developing this project to view some schematics that I created in the early 1990's using
Draftsman-EE version 4.09 running on MS-DOS. 

Draftsman-EE was a PC-based schematic editor and PCB layout software package by
Design Computation, Inc. It was offered from the late 1980's to early 1990's.
Both the software and thecompany went away before the early days of the internet.
Also, the software was also not widely advertised. Consequently, internet searches turn up
almost zero references the company or its Draftsman-EE software, let alone any publicly available
archives of the software.

##  Overview of the software
The Draftsman-EE schematic files, and much of the associated library files, consist of
lines of printable ASCII text. The text present a series of "command lines" that indicate how lines and text should be drawn, including
parameters such as coordinates, color, and scaling factors, along with whether additional 'modules' should be included from another file
or library. The viewer program parses these commands and their parameters in order to draw the content to the screen. 

## DEVELOPMENT ENVIRONMENT AND EXECUTABLES
The project is presently being developed and tested as a Windows "Forms" application using C#
and .NET Framework 4.6.1 using Microsoft Visual Studio Community Edition, 2017 on Windows 10 64-bit.
