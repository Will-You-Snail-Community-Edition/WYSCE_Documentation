---
permalink: /functions/modhelper_createprop_help
---
# modhelper_createprop_help  
&nbsp;  
# Description  
Returns an tool property (used to display a "help/info" message giving info about the object) to be added to an editor object in leveleditor_database_ini or modhelper_predatabase_override_me 
&nbsp;  
# Arguments
### key
The identifier the editor object is saved/loaded/referred to as internally
&nbsp;    
### loca_string
The id of the NAME of the property in the localization spreadsheet. In Community Edition putting a string that ISN'T a localization ID will treat the id AS the string. In vanilla it returns an "invalid loca string" variant.
&nbsp;    
### icon
The icon of the property in the editor settings UI
&nbsp;    
### looltip_loca_string
The id of the TOOL TIP of the property in the localization spreadsheet. In Community Edition putting a string that ISN'T a localization ID will treat the id AS the string. In vanilla it returns an "invalid loca string" variant.
&nbsp;    


