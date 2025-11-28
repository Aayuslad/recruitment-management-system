import { useEditDocumentType } from '@/api/document-api';
import { Button } from '@/components/ui/button';
import {
    Dialog,
    DialogClose,
    DialogContent,
    DialogDescription,
    DialogFooter,
    DialogHeader,
    DialogTitle,
} from '@/components/ui/dialog';
import { Input } from '@/components/ui/input';
import { Label } from '@/components/ui/label';
import { useAppStore } from '@/store';
import type { EditDocumentTypeCommandCorrected } from '@/types/document-types';
import { zodResolver } from '@hookform/resolvers/zod';
import { useEffect } from 'react';
import { useForm } from 'react-hook-form';
import { toast } from 'sonner';
import z from 'zod';
import { useShallow } from 'zustand/react/shallow';

const editDocumentTypeFormSchema = z.object({
    id: z.string().nonempty('Document ID is required'),
    name: z
        .string()
        .min(2, 'Name must be at least 2 characters long')
        .max(50, 'Name must be at most 50 characters long'),
}) satisfies z.ZodType<EditDocumentTypeCommandCorrected>;

export function EditDocTypeDialog() {
    const {
        documentTypeEditTarget,
        isDocumentTypeEditDialogOpen,
        closeDocumentTypeEditDialog,
        setDocumentTypeEditDialogOpen,
    } = useAppStore(
        useShallow((s) => ({
            documentTypeEditTarget: s.documentTypeEditTarget,
            isDocumentTypeEditDialogOpen: s.isDocumentTypeEditDialogOpen,
            closeDocumentTypeEditDialog: s.closeDocumentTypeEditDialog,
            setDocumentTypeEditDialogOpen: s.setDocumentTypeEditDialogOpen,
        }))
    );

    const editDocumentTypeMutation = useEditDocumentType();

    const form = useForm<EditDocumentTypeCommandCorrected>({
        resolver: zodResolver(editDocumentTypeFormSchema),
    });

    useEffect(() => {
        form.setValue('id', documentTypeEditTarget?.id || '');
        form.setValue('name', documentTypeEditTarget?.name || '');
    }, [documentTypeEditTarget, form]);

    const onSubmit = async (data: EditDocumentTypeCommandCorrected) => {
        editDocumentTypeMutation.mutate(data, {
            onSuccess: () => {
                form.reset();
                closeDocumentTypeEditDialog();
            },
        });
    };

    const onInvalid = (errors: typeof form.formState.errors) => {
        const messages = Object.values(errors).map((err) => err.message);
        messages.reverse().forEach((msg) => toast.error(msg));
    };

    return (
        <Dialog
            open={isDocumentTypeEditDialogOpen}
            onOpenChange={setDocumentTypeEditDialogOpen}
        >
            <DialogContent className="sm:max-w-[425px]">
                <form
                    onSubmit={form.handleSubmit(onSubmit, onInvalid)}
                    className="grid gap-7"
                >
                    <DialogHeader>
                        <DialogTitle>Edit Document Type</DialogTitle>
                        <DialogDescription>
                            Edit the document type details and click Save.
                        </DialogDescription>
                    </DialogHeader>

                    <div className="grid gap-4">
                        <div className="grid gap-3">
                            <Label htmlFor="name">Name</Label>
                            <Input id="name" {...form.register('name')} />
                        </div>
                    </div>

                    <DialogFooter>
                        <DialogClose asChild>
                            <Button
                                variant="outline"
                                disabled={editDocumentTypeMutation.isPending}
                            >
                                Cancel
                            </Button>
                        </DialogClose>
                        <Button
                            type="submit"
                            disabled={editDocumentTypeMutation.isPending}
                        >
                            Save
                        </Button>
                    </DialogFooter>
                </form>
            </DialogContent>
        </Dialog>
    );
}
