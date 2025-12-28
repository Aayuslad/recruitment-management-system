import { useVerifyCandidateBg } from '@/api/candidate-api';
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
import { zodResolver } from '@hookform/resolvers/zod';
import { useEffect, useState } from 'react';
import { useForm } from 'react-hook-form';
import { z } from 'zod';

const verifyCandidateBgFormSchema = z.object({
    candidateId: z.string().nonempty('Candidate ID is required'),
});

type Props = {
    candidateId: string;
};

export function VerifyCandidateBg({ candidateId }: Props) {
    const [open, setOpen] = useState(false);
    const verifyCandidateBgMutation = useVerifyCandidateBg();

    const form = useForm<z.infer<typeof verifyCandidateBgFormSchema>>({
        resolver: zodResolver(verifyCandidateBgFormSchema),
    });

    useEffect(() => {
        form.setValue('candidateId', candidateId);
    }, [candidateId]);

    const onSubmit = async (data: { candidateId: string }) => {
        verifyCandidateBgMutation.mutate(data.candidateId, {
            onSuccess: () => {
                form.reset();
                setOpen(false);
            },
        });
    };

    return (
        <Dialog open={open} onOpenChange={setOpen}>
            <DialogTrigger asChild>
                <Button variant="outline">Verify Background</Button>
            </DialogTrigger>
            <DialogContent className="sm:max-w-[425px]">
                <form
                    onSubmit={form.handleSubmit(onSubmit)}
                    className="space-y-4"
                >
                    <DialogHeader>
                        <DialogTitle>Verify Candidate Background</DialogTitle>
                        <DialogDescription>
                            Sure you want to mark this candidate as verified ?
                        </DialogDescription>
                    </DialogHeader>

                    <DialogFooter>
                        <DialogClose asChild>
                            <Button
                                variant="outline"
                                disabled={verifyCandidateBgMutation.isPending}
                            >
                                Cancel
                            </Button>
                        </DialogClose>
                        <Button
                            type="submit"
                            disabled={verifyCandidateBgMutation.isPending}
                        >
                            Verify
                        </Button>
                    </DialogFooter>
                </form>
            </DialogContent>
        </Dialog>
    );
}
