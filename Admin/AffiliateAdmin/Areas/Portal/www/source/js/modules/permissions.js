export const permissionResource = {
    AffReport: "[AffReport]",
    TransferPlayerSetting: "[HideTransferPlayerSetting]", //Hide Transfer Player
    TransferPlayerReport: "[HideTransferPlayerReport]" //Hide Transfer Player
}

export class Permission {
    constructor(permissions) {
        this.permissions = permissions
    }

    static get currentPermissions() {
        const { permissions } = window.globalVariables || { permissions: [] };

        return new Permission(permissions);
    }

    getPermissionInfo = (functionName) => {
        return this.permissions.find(p => p.FunctionName === functionName);
    }

    hasPermission = (functionName) => !!this.getPermissionInfo(functionName)
}