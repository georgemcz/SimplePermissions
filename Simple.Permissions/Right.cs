using System;

namespace Simple.Permissions
{
    /// <summary>
    /// Definition of the entity permissions
    /// </summary>
    [Flags]
    public enum Right
    {
        /// <summary>
        /// No permissions at all.
        /// </summary>
        None = 0x00000000,

        /// <summary>
        /// The view permission. Entity can be viewed.
        /// </summary>
        View = 0x00000001,

        /// <summary>
        /// The delete permission. Entity can be deleted.
        /// </summary>
        Delete = 0x00000002,

        /// <summary>
        /// The create in container permission.
        /// Valid for containers only.
        /// New item can be created in such a container.
        /// </summary>
        CreateInContainer = 0x00000004,

        /// <summary>
        /// The change parent permission. Entity can be moved in a tree.
        /// </summary>
        ChangeParent = 0x00000008, // this is the same as Modify only, but I keep it for brevity

        /// <summary>
        /// The modify permission. Entity can be modified.
        /// </summary>
        Modify = 0x00000010,

        /// <summary>
        /// The modify permissions. Entity ACL can be modified.
        /// </summary>
        ModifyPermissions = 0x00002000,

        /// <summary>
        /// To assign to the group - used in roles, access levels and other permission modifying groups.
        /// </summary>
        AssignTo = 0x00004000,

        /// <summary>
        /// All available permissions
        /// </summary>
        FullControl = View | Delete | CreateInContainer | ChangeParent
                      | Modify | ModifyPermissions | AssignTo
    }
}