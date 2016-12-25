<?xml version="1.0" encoding="UTF-8" ?>
<!DOCTYPE xsl:stylesheet [
	<!ENTITY nbsp "&#160;">
]>
<xsl:stylesheet
	version="1.0"
	xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
	xmlns:XsltFormatUtils="urn:XsltFormatUtils"
	>

	<xsl:output
		method="html"
		version="4.0"
		encoding="utf-8"
		omit-xml-declaration="yes"
		standalone="no"
		doctype-public="-//W3C//DTD HTML 4.0 Transitional//EN"
		indent="yes"
		/>

	<xsl:template match="/">
		<html>
			<head>
				<meta http-equiv="X-UA-Compatible" content="IE=edge"/>
				<title>
					<xsl:value-of select="//Message/Subject" />
				</title>
				<link href="janus://style/forum.css" rel="stylesheet" type="text/css" />
				<link href="janus://formatter/formatter.css" rel="stylesheet" type="text/css" />
				<link href="janus://style/janus.css" rel="stylesheet" type="text/css" />
				<script src="janus://formatter/formatter.js"></script>
			</head>
			<body marginwidth="0" marginheight="0">
				<xsl:apply-templates select="Message" />
				<script type="text/javascript">
					<xsl:value-of select="XsltFormatUtils:FriendlyCopyCode()" 
									disable-output-escaping="yes"/>
					
					<xsl:value-of select="XsltFormatUtils:MediaContentCode()"
									disable-output-escaping="yes"/>
				</script>
			</body>
		</html>
	</xsl:template>

	<xsl:template match="Message">

		<xsl:if test="FormattingOptions/ShowHeader = 'true'">

			<table class="header" width="100%" border="0" cellspacing="0">
				<tbody>
					<tr>
						<td nowrap="nowrap" width="33%">
							<xsl:text>&nbsp;</xsl:text>
							<a>
								<xsl:attribute name="href">
									<xsl:value-of select="XsltFormatUtils:FormatUserInfoURI(Author/ID)" />
								</xsl:attribute>

								<img class="himg">

									<xsl:attribute name="src">
										<xsl:value-of select="XsltFormatUtils:GetUserImagePath(Author/UserClass)"/>
									</xsl:attribute>

									<xsl:attribute name="alt">
										<xsl:value-of select="Author/DisplayName"/>
									</xsl:attribute>

								</img>
							</a>

							<xsl:text>&nbsp;</xsl:text>

							<a>
								<xsl:attribute name="href">
									<xsl:value-of select="XsltFormatUtils:FormatUserInfoURI(Author/ID)" />
								</xsl:attribute>
								<b>
									<xsl:value-of select="Author/DisplayName" />
								</b>
							</a>
							<xsl:value-of disable-output-escaping="yes"
											select="XsltFormatUtils:FormatUserClass(Author/UserClass)" />
						</td>
						<td nowrap="nowrap" width="17%">
							<xsl:text>&nbsp;</xsl:text>

							<img>
								<xsl:attribute name="src">
									<xsl:value-of select="XsltFormatUtils:GetWeekDayImagePath(Date/DayOfWeek, Date/IsOutdate = 'true')"/>
								</xsl:attribute>
							</img>

							<xsl:text>&nbsp;</xsl:text>

							<span>
								<xsl:if test="Date/IsOutdate = 'true'">
									<xsl:attribute name="class">
										<xsl:text>outdate</xsl:text>
									</xsl:attribute>
								</xsl:if>
								<xsl:value-of select="Date/Value" />
							</span>
						</td>

						<td nowrap="nowrap" width="17%">

							<xsl:text>&nbsp;</xsl:text>

							<xsl:if test="Rate/Summary != ''">
								<a>
									<xsl:attribute name="href">
										<xsl:value-of select="XsltFormatUtils:FormatMessageRateURI(ID)" />
									</xsl:attribute>

									<img class="himg">
										<xsl:attribute name="src">
											<xsl:value-of select="XsltFormatUtils:GetConstSizeImagePath('RateGroup')" />
										</xsl:attribute>
										<xsl:attribute name="alt">
											<xsl:value-of select="Rate/Summary" />
										</xsl:attribute>
									</img>
								</a>

								<xsl:text>&nbsp;</xsl:text>

								<a>
									<xsl:attribute name="href">
										<xsl:value-of select="XsltFormatUtils:FormatMessageRateURI(ID)" />
									</xsl:attribute>
									<b>
										<xsl:value-of select="Rate/Summary" />
									</b>
								</a>
							</xsl:if>
						</td>
						<td nowrap="nowrap" width="33%">
							<xsl:if test="Name != ''">
								<xsl:text>&nbsp;</xsl:text>

								<img>
									<xsl:attribute name="src">
										<xsl:value-of select="XsltFormatUtils:GetConstSizeImagePath('NameGroup')" />
									</xsl:attribute>
								</img>

								<xsl:text>&nbsp;</xsl:text>

								<xsl:value-of select="Name"
												disable-output-escaping="yes" />
							</xsl:if>
						</td>
					</tr>
					<tr>
						<td nowrap="nowrap"
							colspan="4">
							<xsl:text>&nbsp;</xsl:text>

							<img style="vertical-align: middle">
								<xsl:attribute name="src">
									<xsl:value-of select="XsltFormatUtils:GetMessageImagePath(IsUnread = 'false', IsMarked = 'true', number(ArticleID) > 0, ViolationPenaltyType , ViolationReason)" />
								</xsl:attribute>
								<xsl:if test="ViolationReason != ''">
									<xsl:attribute name="alt">
										<xsl:value-of select="ViolationReason" />
									</xsl:attribute>
									<xsl:attribute name="title">
										<xsl:value-of select="ViolationReason" />
									</xsl:attribute>
								</xsl:if>
							</img>

							<xsl:text>&nbsp;</xsl:text>
							<xsl:value-of select="Subject"/>
						</td>
					</tr>
				</tbody>
			</table>

		</xsl:if>

		<div id="MsgBody">

			<div class="m">

				<xsl:if test="FormattingOptions/ShowRateFrame = 'true' and count(Rate/RateList/RateItem) &gt; 0">
					<xsl:apply-templates select="Rate"/>
				</xsl:if>

				<xsl:value-of select="Content" disable-output-escaping="yes" />

				<xsl:apply-templates select="Tags"/>

				<div class="o">
					<xsl:value-of select="Origin" disable-output-escaping="yes"/>
				</div>
			</div>
		</div>

	</xsl:template>

	<xsl:template match="Rate">

		<div class="rate-box">
			<table cellspacing="0" class="smallrate">
				<tbody>
					<tr>
						<td colspan="2"
							align="center"
							class="hdr">
							<a>
								<xsl:attribute name="href">
									<xsl:value-of select="XsltFormatUtils:FormatMessageRateURI(//Message/ID)" />
								</xsl:attribute>
								<b>
									<xsl:value-of select="Summary" />
								</b>
							</a>
						</td>
					</tr>
					<xsl:for-each select="RateList/RateItem">
						<tr>
							<xsl:attribute name="class">
								<xsl:if test="position() mod 2 = 0">
									<xsl:text>even</xsl:text>
								</xsl:if>
								<xsl:if test="position() mod 2 = 1">
									<xsl:text>uneven</xsl:text>
								</xsl:if>
							</xsl:attribute>
							<td nowrap="nowrap"
								style="text-align:right;">
								<xsl:if test="Value != '' and ForumInTop = 'true'">

									<b>
										<xsl:value-of select="Value" />
									</b>
									<xsl:text> x </xsl:text>

								</xsl:if>

								<xsl:if test="Value != '' and ForumInTop = 'false'">

									<xsl:value-of select="Value" />
									<xsl:text> x </xsl:text>

								</xsl:if>

								<img class="himg">
									<xsl:attribute name="src">
										<xsl:value-of select="XsltFormatUtils:GetRateImagePath(Type)"/>
									</xsl:attribute>

									<xsl:attribute name="alt">
										<xsl:value-of select="Value"/>
									</xsl:attribute>
								</img>
							</td>
							<td nowrap="nowrap">
								<xsl:text>&nbsp;</xsl:text>
								<a>
									<xsl:attribute name="href">
										<xsl:value-of select="XsltFormatUtils:FormatUserInfoURI(Author/ID)" />
									</xsl:attribute>
									<img class="himg">

										<xsl:attribute name="src">
											<xsl:value-of select="XsltFormatUtils:GetUserImagePath(Author/UserClass)"/>
										</xsl:attribute>

										<xsl:attribute name="alt">
											<xsl:value-of select="Author/DisplayName"/>
										</xsl:attribute>

									</img>
								</a>

								<xsl:text>&nbsp;</xsl:text>

								<a>
									<xsl:attribute name="href">
										<xsl:value-of select="XsltFormatUtils:FormatUserInfoURI(Author/ID)" />
									</xsl:attribute>
									<b>
										<xsl:value-of select="Author/DisplayName" />
									</b>
								</a>
							</td>
						</tr>
					</xsl:for-each>
				</tbody>
			</table>
		</div>

	</xsl:template>

	<xsl:template match="Tags">
		<div class="tags-box">
			<xsl:for-each select="string">
				<a class="tag" href="">
					<xsl:attribute name="href">
						<xsl:value-of select="XsltFormatUtils:FormatTagURI(text())"/>
					</xsl:attribute>
					<xsl:value-of select="text()"/>
				</a>
			</xsl:for-each>
		</div>
	</xsl:template>

</xsl:stylesheet>
