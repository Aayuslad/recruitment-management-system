import { EmployeesTable } from '@/components/admin/employees/employees-table';

export function EmployeesPage() {
    return (
        <div className="h-full">
            <div className="bg-muted dark:bg-slate-800/40 h-[100px] w-full flex items-center">
                <div className="px-10 flex-1">
                    <h1 className="text-2xl font-bold">Employees</h1>
                    <p>List of all employees registered within the platform.</p>
                </div>
                <div className="w-[200px] mb-4"></div>
            </div>

            <div className="w-full flex justify-center pt-10">
                <div className="mr-12">
                    <EmployeesTable />
                </div>
            </div>
        </div>
    );
}
