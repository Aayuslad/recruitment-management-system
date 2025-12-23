import { ManageUserRolesDialog } from '@/components/admin/users/manage-user-roles-dialog';
import { UsersTable } from '@/components/admin/users/users-table';

export function UsersPage() {
    return (
        <div className="h-full">
            <div className="bg-muted dark:bg-slate-800/40 h-[100px] w-full flex items-center">
                <div className="px-10 flex-1">
                    <h1 className="text-2xl font-bold">Users</h1>
                    <p>Manage all platform users and their roles.</p>
                </div>
                <div className="w-[200px] mb-4"></div>
            </div>

            <div className="w-full flex justify-center pt-10">
                <div className="mr-12">
                    <UsersTable />
                </div>

                <ManageUserRolesDialog />
            </div>
        </div>
    );
}
