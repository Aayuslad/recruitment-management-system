import { CreatePositionBatchSheet } from '@/components/position/create-position-batch-seet';
import { PositionBatchesTable } from '@/components/position/position-batches-table';

export function Index() {
    return (
        <div className=" h-full">
            <div className="bg-muted dark:bg-slate-800/40 h-[100px] w-full flex items-center">
                <div className="px-10 flex-1">
                    <h1 className="text-2xl font-bold">Positions</h1>
                    <p>
                        Manage all open and closed recruitment positions grouped
                        by batches.
                    </p>
                </div>
                <div className="w-[230px] mb-4">
                    <CreatePositionBatchSheet
                        visibleTo={['Admin', 'Recruiter']}
                    />
                </div>
            </div>

            <div className="w-full flex justify-evenly pt-10">
                <div className="w-full flex flex-col items-center">
                    <div className="mr-12 w-fit">
                        <PositionBatchesTable />
                    </div>
                </div>
            </div>
        </div>
    );
}
