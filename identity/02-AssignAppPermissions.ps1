param(
    [Parameter(Mandatory=$True, Position=0, HelpMessage="The name of the Azure Subscription")]
    [string]$Subscription,
    [Parameter(Mandatory=$True, Position=1, HelpMessage="The Application ObjectId")]
    [string]$ObjectId
)

Add-MsolRoleMember -RoleObjectId 88d8e3e3-8f55-4a1e-953a-9b9898b8876b -RoleMemberObjectId $ObjectId -RoleMemberType servicePrincipal
Add-MsolRoleMember -RoleObjectId 9360feb5-f418-4baa-8175-e2a00bac4301 -RoleMemberObjectId $ObjectId -RoleMemberType servicePrincipal
Add-MsolRoleMember -RoleObjectId fe930be7-5e62-47db-91af-98c3a49a38b1 -RoleMemberObjectId $ObjectId -RoleMemberType servicePrincipal
