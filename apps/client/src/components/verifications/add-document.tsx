import { useAddDocument } from '@/api/candidate-api';
import { Button } from '@/components/ui/button';
import {
    Dialog,
    DialogClose,
    DialogContent,
    DialogDescription,
    DialogFooter,
    DialogHeader,
    DialogTitle,
    DialogTrigger,
} from '@/components/ui/dialog';
import { Input } from '@/components/ui/input';
import { Label } from '@/components/ui/label';
import type { AddCandidateDocumentCommandCorrected } from '@/types/candidate-types';
import { zodResolver } from '@hookform/resolvers/zod';
import { useEffect, useState } from 'react';
import { useForm } from 'react-hook-form';
import { toast } from 'sonner';
import { z } from 'zod';
import { DocumentTypeSelector } from './internal/document-type-selector';

const addCandidateDocumentFormSchema = z.object({
    id: z.string().nonempty('Candidate ID is required'),
    documentTypeId: z.string().nonempty('Document type ID is required'),
    url: z.string().nonempty('Document URL is required'),
}) satisfies z.ZodType<AddCandidateDocumentCommandCorrected>;

type Props = {
    candidateId: string;
};

export function AddDocument({ candidateId }: Props) {
    const [open, setOpen] = useState(false);
    const addCandidateDocumentMutation = useAddDocument();

    const form = useForm<AddCandidateDocumentCommandCorrected>({
        resolver: zodResolver(addCandidateDocumentFormSchema),
    });

    useEffect(() => {
        form.setValue('id', candidateId);
    }, [candidateId, form]);

    const onSubmit = async (data: AddCandidateDocumentCommandCorrected) => {
        addCandidateDocumentMutation.mutate(data, {
            onSuccess: () => {
                form.reset();
                setOpen(false);
            },
        });
    };

    const onInvalid = (errors: typeof form.formState.errors) => {
        const messages = Object.values(errors).map((err) => err.message);
        messages.reverse().forEach((msg) => toast.error(msg));
    };

    return (
        <Dialog open={open} onOpenChange={setOpen}>
            <DialogTrigger asChild>
                <Button variant="outline">+ Add Document</Button>
            </DialogTrigger>
            <DialogContent className="sm:max-w-[425px]">
                <form
                    onSubmit={form.handleSubmit(onSubmit, onInvalid)}
                    className="space-y-3"
                >
                    <DialogHeader>
                        <DialogTitle>Add Document</DialogTitle>
                        <DialogDescription>
                            select a document. Click submit when you&apos;re
                            done.
                        </DialogDescription>
                    </DialogHeader>
                    <div className="grid gap-4">
                        <div className="grid gap-3">
                            <DocumentTypeSelector
                                selectedTypeId={form.watch('documentTypeId')}
                                setSelectedTypeId={(typeId) =>
                                    form.setValue('documentTypeId', typeId)
                                }
                            />
                        </div>
                        <div className="grid gap-3">
                            <Label htmlFor="doc-url">Document URL</Label>
                            <Input id="doc-url" {...form.register('url')} />
                        </div>
                    </div>
                    <DialogFooter>
                        <DialogClose asChild>
                            <Button
                                variant="outline"
                                disabled={
                                    addCandidateDocumentMutation.isPending
                                }
                            >
                                Cancel
                            </Button>
                        </DialogClose>
                        <Button
                            type="submit"
                            disabled={addCandidateDocumentMutation.isPending}
                        >
                            Submit
                        </Button>
                    </DialogFooter>
                </form>
            </DialogContent>
        </Dialog>
    );
}
