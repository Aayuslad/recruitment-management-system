import { RolesTable } from '@/components/admin/roles/roles-table';

export function RolesPage() {
    return (
        <div className="h-full">
            <div className="bg-muted dark:bg-slate-800/40 h-[100px] w-full flex items-center">
                <div className="px-10 flex-1">
                    <h1 className="text-2xl font-bold">Roles</h1>
                    <p>List of all roles available within the platform.</p>
                </div>
                <div className="w-[200px] mb-4"></div>
            </div>

            <div className="w-full flex justify-center pt-10">
                <div className="mr-12">
                    <RolesTable />
                </div>
            </div>
        </div>
    );
}
