import { CreateDocTypeDialog } from '@/components/configurations/documentTypes/create-doc-type-dialog';
import { DeleteDocTypeDialog } from '@/components/configurations/documentTypes/delete-doc-type-dialog';
import { DocTypesTable } from '@/components/configurations/documentTypes/doc-types-table';
import { EditDocTypeDialog } from '@/components/configurations/documentTypes/edit-doc-type-dialog';

export function DocumentTypesPage() {
    return (
        <div className="h-full">
            <div className="bg-muted dark:bg-slate-800/40 h-[100px] w-full flex items-center">
                <div className="px-10 flex-1">
                    <h1 className="text-2xl font-bold">Document Types</h1>
                    <p>
                        Manage all document types required for candidates,
                        employees, and job applications.
                    </p>
                </div>
                <div className="w-[250px] mb-4">
                    <CreateDocTypeDialog visibleTo={['Admin']} />
                </div>
            </div>

            <div className="w-full flex justify-center pt-10">
                <div className="mr-12">
                    <DocTypesTable />
                </div>
                <EditDocTypeDialog />
                <DeleteDocTypeDialog />
            </div>
        </div>
    );
}
