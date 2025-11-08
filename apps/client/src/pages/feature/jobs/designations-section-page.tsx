import { CreateDesignationDialog } from '@/components/features/jobs/create-designation-dialog';
import { DesignationsTable } from '@/components/features/jobs/designations-table';

function DesinationSectionPage() {
    return (
        <div className=" h-full">
            <div className="bg-popover dark:bg-popover h-[100px] w-full flex items-center">
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
                <DesignationsTable />

                <div className="border w-[300px]">
                    <h2>Audit / Designation details</h2>
                </div>

                {/* <EditSkillDialog /> */}
            </div>
        </div>
    );
}

export default DesinationSectionPage;
