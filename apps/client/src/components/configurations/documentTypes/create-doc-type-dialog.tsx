import { useCreateDocumentType } from '@/api/document-api';
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
import type { CreateDocumentTypeCommandCorrected } from '@/types/document-types';
import { zodResolver } from '@hookform/resolvers/zod';
import { useState } from 'react';
import { useForm } from 'react-hook-form';
import { toast } from 'sonner';
import z from 'zod';

const createDocumentTypeFormSchema = z.object({
    name: z
        .string()
        .min(2, 'Name must be at least 2 characters long')
        .max(50, 'Name must be at most 50 characters long'),
}) satisfies z.ZodType<CreateDocumentTypeCommandCorrected>;

export function CreateDocTypeDialog() {
    const [open, setOpen] = useState(false);
    const createDocumentTypeMutation = useCreateDocumentType();

    const form = useForm<CreateDocumentTypeCommandCorrected>({
        resolver: zodResolver(createDocumentTypeFormSchema),
        defaultValues: {
            name: '',
        },
    });

    const onSubmit = async (data: CreateDocumentTypeCommandCorrected) => {
        createDocumentTypeMutation.mutate(data, {
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
                <Button variant="secondary" className="border">
                    Create Document Type
                </Button>
            </DialogTrigger>

            <DialogContent className="sm:max-w-[425px]">
                <form
                    onSubmit={form.handleSubmit(onSubmit, onInvalid)}
                    className="grid gap-7"
                >
                    <DialogHeader>
                        <DialogTitle>Create Document Type</DialogTitle>
                        <DialogDescription>
                            Enter the document type details and click Create.
                        </DialogDescription>
                    </DialogHeader>

                    <div className="grid gap-4">
                        <div className="grid gap-3">
                            <Label htmlFor="name">Name</Label>
                            <Input
                                id="name"
                                placeholder="Passport"
                                {...form.register('name')}
                            />
                        </div>
                    </div>

                    <DialogFooter>
                        <DialogClose asChild>
                            <Button
                                variant="outline"
                                disabled={createDocumentTypeMutation.isPending}
                            >
                                Cancel
                            </Button>
                        </DialogClose>
                        <Button
                            type="submit"
                            disabled={createDocumentTypeMutation.isPending}
                        >
                            Create
                        </Button>
                    </DialogFooter>
                </form>
            </DialogContent>
        </Dialog>
    );
}
