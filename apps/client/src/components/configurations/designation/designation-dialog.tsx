import {
    Dialog,
    DialogContent,
    DialogDescription,
    DialogHeader,
    DialogTitle,
} from '@/components/ui/dialog';
import { Tabs, TabsContent, TabsList, TabsTrigger } from '@/components/ui/tabs';
import { useAppStore } from '@/store';
import { useShallow } from 'zustand/react/shallow';
import { EditDialogBody } from './designation-dialog-edit-body';
import { ViewDialogBody } from './designation-dialog-view-body';

export function DesignationDialog() {
    const { designationDialog, designationDialogTab, setDesignationDialog } =
        useAppStore(
            useShallow((s) => ({
                designationDialog: s.designationDialog,
                designationDialogTab: s.designationDialogTab,
                setDesignationDialog: s.setDesignationDialog,
            }))
        );

    return (
        <Dialog open={designationDialog} onOpenChange={setDesignationDialog}>
            <DialogContent className="w-[800px]">
                <Tabs defaultValue={designationDialogTab}>
                    <DialogHeader className="mb-1">
                        <TabsList>
                            <TabsTrigger value="view">View</TabsTrigger>
                            <TabsTrigger value="edit">Edit</TabsTrigger>
                        </TabsList>
                        <DialogTitle className="hidden">
                            Designation view and edit
                        </DialogTitle>
                        <DialogDescription className="hidden">
                            View and edit a designation
                        </DialogDescription>
                    </DialogHeader>

                    <TabsContent value="view" className="pb-4">
                        <ViewDialogBody />
                    </TabsContent>
                    <TabsContent value="edit">
                        <EditDialogBody />
                    </TabsContent>
                </Tabs>
            </DialogContent>
        </Dialog>
    );
}
