import { CreateDesignationDialog } from '@/components/configurations/designation/create-designation-dialog';
// import { DesignationsTable } from '@/components/configurations/designation/designations-table';

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
                <div className="w-[200px]">
                    <CreateDesignationDialog />
                </div>
            </div>

            <div className="w-full flex justify-evenly pt-10">
                {/* <DesignationsTable /> */}

                {/* <EditSkillDialog /> */}
            </div>
        </div>
    );
}
