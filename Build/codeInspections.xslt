<?xml version="1.0" encoding="utf-8"?>

<!-- origin: https://gist.github.com/st-gwerner/6675196 -->
<xsl:stylesheet version="3.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:output method="html" version="5.0" indent="yes" />

  <!-- input parameters -->
  <xsl:param name="BuildNumber" select="0.1" />
  
  <xsl:key name="IssueTypesLookup" match="/Report/IssueTypes/IssueType" use="@Id" />
  <xsl:key name="ProjectsLookup" match="/Report/Issues/Project" use="@Name" />
  
  <xsl:template match="/Report">
    <html>
      <head>
        <meta name="viewport" content="width=device-width, initial-scale=1.0" />
        <link rel="stylesheet" href="https://netdna.bootstrapcdn.com/bootstrap/3.3.6/css/bootstrap.min.css" />
        <link rel="stylesheet" href="https://netdna.bootstrapcdn.com/bootstrap/3.3.6/css/bootstrap-theme.min.css" />
      </head>
      <body>
        <div class="container">
          <h1>Statistics for build <xsl:value-of select="$BuildNumber"/></h1>
          <h2>Summary by projects</h2>
          <table class="table table-condensed">
            <thead>
              <tr>
                <th>Project</th>
                <th>Number of inspections</th>
              </tr>
            </thead>
            <tbody>
              <xsl:for-each-group select="./Issues/Project" group-by="@Name">
                <tr>
                  <td>
                    <xsl:variable name="projName"><xsl:value-of select="current-grouping-key()" /></xsl:variable>
                    <a href="#{generate-id(key('ProjectsLookup', $projName))}">
                      <!--<xsl:attribute name="href">#<xsl:value-of select="generate-id($projName)"/></xsl:attribute>-->
                      <xsl:value-of select="$projName" />
                    </a>
                  </td>
                  <td><xsl:value-of select="count(current-group()/Issue)" /></td>
                </tr>
              </xsl:for-each-group>
              <tr>
                <th>Total inspections</th>
                <th><xsl:value-of select="count(/Report/Issues//Issue)" /></th>
              </tr>
            </tbody>
          </table>

          <h1>Details by projects</h1>
          <xsl:apply-templates select="./Issues/Project" />
        </div>
        <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery/2.2.0/jquery.min.js" />
        <script src="https://netdna.bootstrapcdn.com/bootstrap/3.3.6/js/bootstrap.min.js" />
      </body>
    </html>
  </xsl:template>


  <xsl:template match="Project">
    <xsl:variable name="projectName"><xsl:value-of select="@Name" /></xsl:variable>
    <hr />
    <h2 id="{generate-id(key('ProjectsLookup', $projectName))}">
      <xsl:value-of select="$projectName" />
    </h2>

    <xsl:for-each-group select="./Issue" group-by="@TypeId">
      <xsl:variable name="issueTypeId" select="@TypeId" />
      <xsl:apply-templates select="key('IssueTypesLookup', @TypeId)" />

      <table class="table table-striped table-condensed table-bordered">
        <thead>
          <tr>
            <th style="width: 30%">File name</th>
            <th style="width: 10%">Line number</th>
            <th style="width: 60%">Message</th>
          </tr>
        </thead>
        <tbody>
          <xsl:for-each select="/Report/Issues/Project[@Name=$projectName]/Issue[@TypeId=$issueTypeId]">
            <tr>
              <td><xsl:value-of select="replace(@File, $projectName, '.')" /></td>
              <td><xsl:value-of select="@Line" /></td>
              <td><xsl:value-of select="@Message" /></td>
            </tr>
          </xsl:for-each>
        </tbody>
      </table>
    </xsl:for-each-group>
  </xsl:template>


  <xsl:template match="IssueType">
    <h3><xsl:value-of select="@Description" /></h3>
    <xsl:element name="div">
      <xsl:choose>
        <xsl:when test="@Severity='WARNING'"><xsl:attribute name="class">text-warning</xsl:attribute></xsl:when>
        <xsl:when test="@Severity='SUGGESTION'"><xsl:attribute name="class">text-info</xsl:attribute></xsl:when>
      </xsl:choose>
      <p><xsl:value-of select="@Category" />
      <xsl:if test="@WikiUrl != ''">
        <span>, </span>
        <a href="{@WikiUrl}">Wiki article</a>
      </xsl:if>
      </p>
    </xsl:element>
  </xsl:template>

</xsl:stylesheet>