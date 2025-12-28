import { useVerifyDocument } from '@/api/candidate-api';
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
import type { VerifyCandidateDocumentCommandCorrected } from '@/types/candidate-types';
import { zodResolver } from '@hookform/resolvers/zod';
import { useEffect, useState } from 'react';
import { useForm } from 'react-hook-form';
import { toast } from 'sonner';
import { z } from 'zod';

const verifyCandidateDocumentFormSchema = z.object({
    candidateId: z.string().nonempty('Candidate ID is required'),
    documentId: z.string().nonempty('Document type ID is required'),
}) satisfies z.ZodType<VerifyCandidateDocumentCommandCorrected>;

type Props = {
    candidateId: string;
    documentId: string;
};

export function VerifyDocumentButton({ candidateId, documentId }: Props) {
    const [open, setOpen] = useState(false);
    const verifyCandidateDocMutation = useVerifyDocument();

    const form = useForm<z.infer<typeof verifyCandidateDocumentFormSchema>>({
        resolver: zodResolver(verifyCandidateDocumentFormSchema),
    });

    useEffect(() => {
        form.setValue('candidateId', candidateId);
        form.setValue('documentId', documentId);
    }, [candidateId, documentId]);

    const onSubmit = async (data: VerifyCandidateDocumentCommandCorrected) => {
        verifyCandidateDocMutation.mutate(data, {
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
                <Button
                    variant="secondary"
                    className="h-7 hover:cursor-pointer"
                >
                    Verify
                </Button>
            </DialogTrigger>
            <DialogContent className="sm:max-w-[425px]">
                <form
                    onSubmit={form.handleSubmit(onSubmit, onInvalid)}
                    className="space-y-4"
                >
                    <DialogHeader>
                        <DialogTitle>Verify the document</DialogTitle>
                        <DialogDescription>
                            Sure you want to mark this document as verified ?
                        </DialogDescription>
                    </DialogHeader>

                    <DialogFooter>
                        <DialogClose asChild>
                            <Button
                                variant="outline"
                                disabled={verifyCandidateDocMutation.isPending}
                            >
                                Cancel
                            </Button>
                        </DialogClose>
                        <Button
                            type="submit"
                            disabled={verifyCandidateDocMutation.isPending}
                        >
                            Verify
                        </Button>
                    </DialogFooter>
                </form>
            </DialogContent>
        </Dialog>
    );
}
