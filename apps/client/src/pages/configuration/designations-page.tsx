import { CreateDesignationDialog } from '@/components/configurations/designation/create-designation-dialog';
import { DeleteDesignationDialog } from '@/components/configurations/designation/delete-designation-dialog';
import { DesignationsTable } from '@/components/configurations/designation/designations-table';
import { DesignationDialog } from '@/components/configurations/designation/designation-dialog';

export function DesignationsPage() {
    return (
        <div className=" h-full">
            <div className="bg-muted dark:bg-slate-800/40 h-[100px] w-full flex items-center">
                <div className="px-10 flex-1">
                    <h1 className="text-2xl font-bold">Designations</h1>
                    <p>
                        Manage all organizational designations and their
                        required skills.
                    </p>
                </div>
                <div className="w-[230px] mb-4">
                    <CreateDesignationDialog />
                </div>
            </div>

            <div className="w-full flex justify-evenly pt-10">
                <div className="mr-12">
                    <DesignationsTable />
                </div>

                <DeleteDesignationDialog />
                <DesignationDialog />
            </div>
        </div>
    );
}
