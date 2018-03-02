<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:wix="http://schemas.microsoft.com/wix/2006/wi" xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl">

  <xsl:output method="xml" version="1.0" encoding="UTF-8" indent="yes" />

  <xsl:template match="@*|node()">
    <xsl:copy>
      <xsl:apply-templates select="@*|node()"/>
    </xsl:copy>
  </xsl:template>

  <xsl:key name="exe-search" match="wix:Component[wix:File/@Id='BlackMirror.Sync.Worker.Host.exe']" use="@Id"/>
  <xsl:key name="config-search" match="wix:Component[wix:File/@Id='BlackMirror.Sync.Worker.Host.exe.config']" use="@Id"/>
  <xsl:variable name="in64">x64</xsl:variable>
  <xsl:variable name="out64">p64</xsl:variable>
  <xsl:variable name="in86">x86</xsl:variable>
  <xsl:variable name="out86">p86</xsl:variable>
  
  

  <xsl:template match="wix:Component[key('exe-search', @Id)]"/>
  <xsl:template match="wix:ComponentRef[key('exe-search', @Id)]"/>
  <xsl:template match="wix:Component[key('config-search', @Id)]"/>
  <xsl:template match="wix:ComponentRef[key('config-search', @Id)]"/>
  
   <xsl:template match="@Id">
     <xsl:attribute name="Id">
       <xsl:value-of select="translate(., $in64, $out64)"/>
     </xsl:attribute>
   </xsl:template>
   <xsl:template match="@Id">
     <xsl:attribute name="Id">
       <xsl:value-of select="translate(., $in86, $out86)"/>
     </xsl:attribute>
   </xsl:template>
   
   <xsl:template match="@Directory">
     <xsl:attribute name="Directory">
       <xsl:value-of select="translate(., $in64, $out64)"/>
     </xsl:attribute>
   </xsl:template>
   <xsl:template match="@Directory">
     <xsl:attribute name="Directory">
       <xsl:value-of select="translate(., $in86, $out86)"/>
     </xsl:attribute>
   </xsl:template>
  
  

  

</xsl:stylesheet>
