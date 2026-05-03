---
permalink: /functions/modhelper_createprop_float
---
# modhelper_createprop_float  
&nbsp;  
# Description  
Returns a FLOAT (decimal number) tool property to be added to an editor object in leveleditor_database_ini or modhelper_predatabase_override_me 
&nbsp;  
# Arguments
### key
The identifier the editor object is saved/loaded/referred to as internally
&nbsp;    
### copy_property_to_placed_obj
Whether or not to copy the property to a newly placed object
&nbsp;    
### f_min
The minimum value of the property
&nbsp;    
### f_max
The maximum value of the property
&nbsp;    
### f_scroll_steps
The amount that the slider in the editor's settings UI steps by
&nbsp;    
### multiply_for_display
How much the real value is multiplied before it is shown to the user in the settings UI. Useful to show an entire tile (60 pixels) as a 1 value.
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


