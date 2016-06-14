#!/bin/bash
mcs src/*.cs src/*/*.cs -debug+ -optimize+ -out:OutOfAfrica.exe \
  -r:devil/DevIL.NET2 \
  -r:Microsoft.CSharp \
  -r:System.Core \
  -r:System.Data \
  -r:System.Data.DataSetExtensions \
  -r:System.Drawing \
  -r:System.Management \
  -r:System.Windows.Forms \
  -r:System.Xml.Linq
