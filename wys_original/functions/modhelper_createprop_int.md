---
permalink: /functions/modhelper_createprop_int
---
# modhelper_createprop_int  
&nbsp;  
# Description  
Returns an INTEGER tool property to be added to an editor object in leveleditor_database_ini or modhelper_predatabase_override_me 
&nbsp;  
# Arguments
### key
The identifier the editor object is saved/loaded/referred to as internally
&nbsp;    
### copy_property_to_placed_obj
Whether or not to copy the property to a newly placed object
&nbsp;    
### int_min
The minimum value of the property
&nbsp;    
### int_max
The maximum value of the property
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


