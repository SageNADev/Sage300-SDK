<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
<xsl:template match="/">
<html>
    <head>
        <link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/bootstrap/4.3.1/css/bootstrap.min.css"
              integrity="sha384-ggOyR0iXCbMQv3Xipma34MD+dH/1fQ784/j6cY/iJTQUOhcWr7x9JvoRxT2MZw1T" crossorigin="anonymous"/>
    </head>
    <body>
        <div class="container">
            <xsl:for-each select="testsuites/testsuite">
                <h2><xsl:value-of select="@name"/></h2>
                <div class="row">
                    <div class="col-2">Number of tests: <xsl:value-of select="@tests"/></div>
                    <div class="col-2">Failures: <xsl:value-of select="@failures"/></div>
                    <div class="col-4">Errors: <xsl:value-of select="@errors"/></div>
                    <div class="col">Timestamp: <xsl:value-of select="@timestamp"/></div>
                </div>
                <xsl:for-each select="testcase">
                    <table class="table" border="2">
                        <tr>
                            <th>Test Case</th>
                            <th>Run Time</th>
                            <th>Timestamp</th>
                        </tr>
                        <xsl:choose>
                            <xsl:when test="*">
                                <tr bgcolor="#EE5757">
                                    <td style="color:#000000"><xsl:value-of select="@name"/></td>
                                    <td style="color:#000000"><xsl:value-of select="@time"/></td>
                                    <td style="color:#000000"><xsl:value-of select="@timestamp"/></td>
                                </tr>
                            </xsl:when>
                            <xsl:otherwise>
                                <tr bgcolor="#E0F2BE">
                                    <td style="color:#374E0C"><xsl:value-of select="@name"/></td>
                                    <td style="color:#374E0C"><xsl:value-of select="@time"/></td>
                                    <td style="color:#374E0C"><xsl:value-of select="@timestamp"/></td>
                                </tr>
                                <tr bgcolor="#F5FFF2">
                                    <td style="color:#374E0C" colspan="3">All tests passed</td>
                                </tr>
                            </xsl:otherwise>
                        </xsl:choose>
                        <xsl:if test="*">
                            <tbody class="labels">
                                <tr class="accordion-toggle collapsed" data-toggle="collapse" data-target="#collapsible">
                                    <th>Test Name</th>
                                    <th>Actual</th>
                                    <th>Expected</th>
                                </tr>
                            </tbody>
                            <tbody class="collapse.show accordian-body" id="collapsible">
                                <xsl:for-each select="failure">
                                    <tr bgcolor="#FFCACA">
                                        <td style="color:#550000"><xsl:value-of select="@message"/></td>
                                        <td style="color:#550000"><xsl:value-of select="actual/@value"/></td>
                                        <td style="color:#550000"><xsl:value-of select="expected/@value"/></td>
                                    </tr>
                                    <tr bgcolor="FFEEEF">
                                        <xsl:call-template name="tokenize">
                                            <xsl:with-param name="text" select="/testsuites/testsuite/system-out"/>
                                            <xsl:with-param name="index" select="count(.|preceding::failure)"/>
                                        </xsl:call-template>
                                    </tr>
                                </xsl:for-each>
                            </tbody>
                        </xsl:if>
                    </table>
                </xsl:for-each>
            </xsl:for-each>
        </div>
    </body>
</html>
</xsl:template>

<xsl:template name="tokenize">
    <xsl:param name="text"/>
    <xsl:param name="delimiter" select="'&#10;['"/>
    <xsl:param name="index"/>
    <xsl:param name="curr" select="0"/>
    <xsl:variable name="token" select="substring-before(concat($text, $delimiter), $delimiter)"/>
    <xsl:if test="normalize-space($token) and $index = $curr">
        <td style="color:#550000" colspan="3">
            <pre><xsl:value-of select="concat($delimiter, $token)"/></pre>
        </td>
    </xsl:if>
    <xsl:if test="contains($text, $delimiter)">
        <xsl:call-template name="tokenize">
            <xsl:with-param name="text" select="substring-after($text, $delimiter)"/>
            <xsl:with-param name="index" select="$index"/>
            <xsl:with-param name="curr" select="$curr+1"/>
        </xsl:call-template>
    </xsl:if>
</xsl:template>

<xsl:template name="tokenize_old">
    <xsl:param name="text"/>
    <xsl:param name="delimiter" select="'&#10;['"/>
    <xsl:param name="index"/>
    <xsl:param name="curr" select="0"/>
    <xsl:variable name="adj-text" select="concat($text, $delimiter)" />
    <xsl:variable name="token">
        <xsl:value-of select="substring-before($adj-text, $delimiter)"/>
        <xsl:value-of select="$delimiter"/>
        <xsl:value-of select="substring-before(substring-after($adj-text, $delimiter), $delimiter)"/>
    </xsl:variable>
    <xsl:if test="normalize-space($token) and $index = $curr">
        <td style="color:#550000" colspan="3">
            <xsl:value-of select="substring-before($token, $delimiter)"/><br/><xsl:value-of select="substring-after($token, $delimiter)"/>
        </td>
    </xsl:if>
    <xsl:variable name="tail" select="substring-after(substring-after($text, $delimiter), $delimiter)"/>
    <xsl:if test="$tail and $index >= $curr">
        <xsl:call-template name="tokenize">
            <xsl:with-param name="text" select="$tail"/>
            <xsl:with-param name="index" select="$index"/>
            <xsl:with-param name="curr" select="$curr+1"/>
        </xsl:call-template>
    </xsl:if>
</xsl:template>

</xsl:stylesheet>