---
permalink: /functions/modhelper_createprop_sprite
---
# modhelper_createprop_sprite  
&nbsp;  
# Description  
Returns a SPRITE/TEXTURE tool property to be added to an editor object in leveleditor_database_ini or modhelper_predatabase_override_me 
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
### default_value
The default value of the property
&nbsp;    
### looltip_loca_string
The id of the TOOL TIP of the property in the localization spreadsheet. In Community Edition putting a string that ISN'T a localization ID will treat the id AS the string. In vanilla it returns an "invalid loca string" variant.
&nbsp;    
### array_sprite_picking_functions
An array of FUNCTIONS that return which sprite is used for each option, NOT a direct array of the sprites themselves.
&nbsp;    


